#region License Information
/* Copyright (C) 2015 Andreas Beham
 *
 * SentenceGenerator is free software: you can redistribute it and/or
 * modify it under the terms of the GNU General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 *
 * SentenceGenerator is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this software. If not, see <http://www.gnu.org/licenses/>.
 */
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SentenceGenerator {
  public class Generator {
    public const int _EOF = 0;
    public const int _ident = 1;
    public const int maxT = 8;

    private const bool _T = true;
    private const bool _x = false;
    private const int minErrDist = 2;

    private Scanner scanner;
    private Errors errors;
    public Errors Errors { get { return errors; } }

    private Token t;    // last recognized token
    private Token la;   // lookahead token
    private int errDist = minErrDist;



    private void Initialize(string str) {
      this.scanner = new Scanner(new MemoryStream(Encoding.UTF8.GetBytes(str)));
      this.errors = new Errors();
      this.Sentences = new List<List<string>>() { new List<string>() };
      this.Stack = new Stack<List<List<string>>>();
      this.Base = new List<List<string>>();
      this.BaseStack = new Stack<List<List<string>>>();
    }

    private List<List<string>> Sentences { get; set; }
    private Stack<List<List<string>>> Stack { get; set; }
    private List<List<string>> Base { get; set; }
    private Stack<List<List<string>>> BaseStack { get; set; }

    public IEnumerable<string> GetSentences(string separator) {
      return Sentences != null ? Sentences.Select(x => string.Join(separator, x)) : Enumerable.Empty<string>();
    }

    private void RecurseDown() {
      Stack.Push(Sentences);
      Sentences = new List<List<string>>() { new List<string>() };
      BaseStack.Push(Base);
      Base = new List<List<string>>();
    }

    private void RecurseUp(bool option) {
      var sentences = Stack.Pop();
      Sentences = (from mine in sentences
                   from theirs in Sentences
                   select mine.Concat(theirs).ToList()).ToList();
      if (option) Sentences.InsertRange(0, sentences);
      Base = BaseStack.Pop();
    }

    private void SynErr(int n) {
      if (errDist >= minErrDist) errors.SynErr(la.line, la.col, n);
      errDist = 0;
    }

    private void Get() {
      for (; ; ) {
        t = la;
        la = scanner.Scan();
        if (la.kind <= maxT) { ++errDist; break; }

        la = t;
      }
    }

    private void Expect(int n) {
      if (la.kind == n) Get(); else { SynErr(n); }
    }

    private bool StartOf(int s) {
      return set[s, la.kind];
    }

    void Process() {
      while (StartOf(1)) {
        if (la.kind == 1) {
          Term();
        } else if (la.kind == 2) {
          Option();
        } else if (la.kind == 4) {
          Alternative();
        } else if (la.kind == 5) {
          Group();
        } else {
          Sequence();
        }
      }
    }

    void Term() {
      Expect(1);
      for (var i = 0; i < Sentences.Count; i++) Sentences[i].Add(t.val);
    }

    void Option() {
      Expect(2);
      RecurseDown();
      Process();
      RecurseUp(true);
      Expect(3);
    }

    void Alternative() {
      Expect(4);
      Base.AddRange(Sentences); Sentences = new List<List<string>>() { new List<string>() };
      if (la.kind == 1) {
        Term();
      } else if (la.kind == 2) {
        Option();
      } else if (la.kind == 5) {
        Group();
      } else SynErr(9);
      Sentences.InsertRange(0, Base); Base = new List<List<string>>();
    }

    void Group() {
      Expect(5);
      RecurseDown();
      Process();
      RecurseUp(false);
      Expect(6);
    }

    void Sequence() {
      Expect(7);
      Process();
    }



    public bool TryParse(string str) {
      Parse(str);
      return Errors.Count == 0;
    }

    public void Parse(string str) {
      Initialize(str);
      la = new Token();
      la.val = "";
      Get();
      Process();
      Expect(0);

    }

    private static readonly bool[,] set = {
    {_T,_x,_x,_x, _x,_x,_x,_x, _x,_x},
    {_x,_T,_T,_x, _T,_T,_x,_T, _x,_x}

    };
  } // end Parser


  public class Errors {
    private int count = 0;                                    // number of errors detected
    public int Count { get { return count; } }
    private TextWriter errorStream = new StringWriter();   // error messages go to this stream
    public string ErrorMessage {
      get { return errorStream.ToString(); }
    }
    private string errMsgFormat = "-- line {0} col {1}: {2}"; // 0=line, 1=column, 2=text

    internal virtual void SynErr(int line, int col, int n) {
      string s;
      switch (n) {
        case 0: s = "EOF expected"; break;
        case 1: s = "ident expected"; break;
        case 2: s = "\"[\" expected"; break;
        case 3: s = "\"]\" expected"; break;
        case 4: s = "\"|\" expected"; break;
        case 5: s = "\"(\" expected"; break;
        case 6: s = "\")\" expected"; break;
        case 7: s = "\";\" expected"; break;
        case 8: s = "??? expected"; break;
        case 9: s = "invalid Alternative"; break;

        default: s = "error " + n; break;
      }
      errorStream.WriteLine(errMsgFormat, line, col, s);
      count++;
    }

    internal virtual void SemErr(int line, int col, string s) {
      errorStream.WriteLine(errMsgFormat, line, col, s);
      count++;
    }

    internal virtual void SemErr(string s) {
      errorStream.WriteLine(s);
      count++;
    }

    internal virtual void Warning(int line, int col, string s) {
      errorStream.WriteLine(errMsgFormat, line, col, s);
    }

    internal virtual void Warning(string s) {
      errorStream.WriteLine(s);
    }
  } // Errors


  [Serializable]
  public class FatalError : Exception {
    public FatalError() { }
    public FatalError(string message) : base(message) { }
    public FatalError(string message, Exception inner) : base(message, inner) { }

    protected FatalError(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context)
      : base(info, context) { }
  }
}