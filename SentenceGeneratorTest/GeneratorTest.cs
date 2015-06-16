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

using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SentenceGenerator;

namespace SentenceGeneratorTest {
  /// <summary>
  /// Summary description for GeneratorTest
  /// </summary>
  [TestClass]
  public class GeneratorTest {

    [TestMethod]
    public void GenerateTest() {
      var generator = new Generator();
      generator.Parse("The [quick] brown fox (jumps | leaps | vaults | springs | pounces | stots | ollies) over the [lazy | irritated] dog");
      var sentences = new HashSet<string>(generator.GetSentences(" "));
      Assert.IsTrue(sentences.Contains("The brown fox jumps over the dog"));
      Assert.IsTrue(sentences.Contains("The brown fox leaps over the dog"));
      Assert.IsTrue(sentences.Contains("The brown fox vaults over the dog"));
      Assert.IsTrue(sentences.Contains("The brown fox springs over the dog"));
      Assert.IsTrue(sentences.Contains("The brown fox pounces over the dog"));
      Assert.IsTrue(sentences.Contains("The brown fox stots over the dog"));
      Assert.IsTrue(sentences.Contains("The brown fox ollies over the dog"));
      Assert.IsTrue(sentences.Contains("The quick brown fox jumps over the dog"));
      Assert.IsTrue(sentences.Contains("The quick brown fox leaps over the dog"));
      Assert.IsTrue(sentences.Contains("The quick brown fox vaults over the dog"));
      Assert.IsTrue(sentences.Contains("The quick brown fox springs over the dog"));
      Assert.IsTrue(sentences.Contains("The quick brown fox pounces over the dog"));
      Assert.IsTrue(sentences.Contains("The quick brown fox stots over the dog"));
      Assert.IsTrue(sentences.Contains("The quick brown fox ollies over the dog"));
      Assert.IsTrue(sentences.Contains("The brown fox jumps over the lazy dog"));
      Assert.IsTrue(sentences.Contains("The brown fox leaps over the lazy dog"));
      Assert.IsTrue(sentences.Contains("The brown fox vaults over the lazy dog"));
      Assert.IsTrue(sentences.Contains("The brown fox springs over the lazy dog"));
      Assert.IsTrue(sentences.Contains("The brown fox pounces over the lazy dog"));
      Assert.IsTrue(sentences.Contains("The brown fox stots over the lazy dog"));
      Assert.IsTrue(sentences.Contains("The brown fox ollies over the lazy dog"));
      Assert.IsTrue(sentences.Contains("The brown fox jumps over the irritated dog"));
      Assert.IsTrue(sentences.Contains("The brown fox leaps over the irritated dog"));
      Assert.IsTrue(sentences.Contains("The brown fox vaults over the irritated dog"));
      Assert.IsTrue(sentences.Contains("The brown fox springs over the irritated dog"));
      Assert.IsTrue(sentences.Contains("The brown fox pounces over the irritated dog"));
      Assert.IsTrue(sentences.Contains("The brown fox stots over the irritated dog"));
      Assert.IsTrue(sentences.Contains("The brown fox ollies over the irritated dog"));
      Assert.IsTrue(sentences.Contains("The quick brown fox jumps over the lazy dog"));
      Assert.IsTrue(sentences.Contains("The quick brown fox leaps over the lazy dog"));
      Assert.IsTrue(sentences.Contains("The quick brown fox vaults over the lazy dog"));
      Assert.IsTrue(sentences.Contains("The quick brown fox springs over the lazy dog"));
      Assert.IsTrue(sentences.Contains("The quick brown fox pounces over the lazy dog"));
      Assert.IsTrue(sentences.Contains("The quick brown fox stots over the lazy dog"));
      Assert.IsTrue(sentences.Contains("The quick brown fox ollies over the lazy dog"));
      Assert.IsTrue(sentences.Contains("The quick brown fox jumps over the irritated dog"));
      Assert.IsTrue(sentences.Contains("The quick brown fox leaps over the irritated dog"));
      Assert.IsTrue(sentences.Contains("The quick brown fox vaults over the irritated dog"));
      Assert.IsTrue(sentences.Contains("The quick brown fox springs over the irritated dog"));
      Assert.IsTrue(sentences.Contains("The quick brown fox pounces over the irritated dog"));
      Assert.IsTrue(sentences.Contains("The quick brown fox stots over the irritated dog"));
      Assert.IsTrue(sentences.Contains("The quick brown fox ollies over the irritated dog"));
      Assert.AreEqual(sentences.Count, 42);
      generator.Parse("An ([optional] alternative | [optional option])");
      sentences = new HashSet<string>(generator.GetSentences(" "));
      Assert.IsTrue(sentences.Contains("An optional alternative"));
      Assert.IsTrue(sentences.Contains("An alternative"));
      Assert.IsTrue(sentences.Contains("An optional option"));
      Assert.IsTrue(sentences.Contains("An"));
      Assert.AreEqual(sentences.Count, 4);
    }
  }
}
