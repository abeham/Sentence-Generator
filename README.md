# Sentence-Generator
Creates all sentences of a given language that expresses sequence and options. The software builds on the compiler generator Coco/R (http://www.ssw.uni-linz.ac.at/Coco/). You will also find the required files for Coco/R here (the .atg and the two .frame files).

The grammar uses "|" to describe alternatives and "[ .. ]" to describe optional elements.

# Usage
```
var generator = new Generator();
generator.Parse("Your [marvellous | humorous | profound] sentence");
var sentences = generator.GetSentences(" ");
// results in
// Your sentence
// Your marvellous sentence
// Your humorous sentence
// Your profound sentence
```

# Example
The following sentence results in a total of 42 concrete expressions:

The [quick] brown fox (jumps | leaps | vaults | springs | pounces | stots | ollies) over the [lazy | irritated] dog
* The brown fox jumps over the dog
* The brown fox leaps over the dog
* The brown fox vaults over the dog
* The brown fox springs over the dog
* The brown fox pounces over the dog
* The brown fox stots over the dog
* The brown fox ollies over the dog
* The quick brown fox jumps over the dog
* The quick brown fox leaps over the dog
* The quick brown fox vaults over the dog
* The quick brown fox springs over the dog
* The quick brown fox pounces over the dog
* The quick brown fox stots over the dog
* The quick brown fox ollies over the dog
* The brown fox jumps over the lazy dog
* The brown fox leaps over the lazy dog
* The brown fox vaults over the lazy dog
* The brown fox springs over the lazy dog
* The brown fox pounces over the lazy dog
* The brown fox stots over the lazy dog
* The brown fox ollies over the lazy dog
* The brown fox jumps over the irritated dog
* The brown fox leaps over the irritated dog
* ...
