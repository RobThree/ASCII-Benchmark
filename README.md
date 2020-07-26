# ASCII-Benchmark
Inspired by https://lemire.me/blog/2020/07/21/avoid-character-by-character-processing-when-performance-matters/

Test strings in the files `allCountries.txt.gz` and `cities500.txt.gz` are [sourced from Geonames.org](https://download.geonames.org/export/dump/) where I have extracted the second column (`name`) of the corresponding files. The `enwik8.gz` file is sourced from [The Large Text Compression Benchmark](http://mattmahoney.net/dc/textdata.html). Testfiles have been gzipped in order to save space. These files have a nice mix of ASCII / non-ASCII data.

# Results

## Intel Core i9-10900X:

    Benching allCountries.txt.gz
            Lines           : 7,522,986
            Avg. length     : 14.58
            Max. length     : 151
            Non-Ascii lines : 23.52 %
    Measuring methods... please be patient...
    Regex           Avg: 1.1677s    Min: 1.1001s    Max: 1.2178s       6,442,421 strings/sec
    Branchy1        Avg: 0.0552s    Min: 0.0497s    Max: 0.0581s     136,181,375 strings/sec
    Branchy2        Avg: 0.0533s    Min: 0.0481s    Max: 0.0615s     141,014,802 strings/sec
    Branchless      Avg: 0.0561s    Min: 0.0519s    Max: 0.0584s     134,095,915 strings/sec
    Hybrid          Avg: 0.0518s    Min: 0.0483s    Max: 0.0553s     145,139,683 strings/sec

    Benching cities500.txt.gz
            Lines           : 165,957
            Avg. length     : 10.14
            Max. length     : 65
            Non-Ascii lines : 20.12 %
    Measuring methods... please be patient...
    Regex           Avg: 0.0224s    Min: 0.0218s    Max: 0.0231s       7,404,511 strings/sec
    Branchy1        Avg: 0.0011s    Min: 0.0008s    Max: 0.0013s     152,820,546 strings/sec
    Branchy2        Avg: 0.0011s    Min: 0.0008s    Max: 0.0013s     152,532,605 strings/sec
    Branchless      Avg: 0.0011s    Min: 0.0009s    Max: 0.0013s     148,741,642 strings/sec
    Hybrid          Avg: 0.0012s    Min: 0.0008s    Max: 0.0015s     142,158,282 strings/sec

    Benching enwik8.gz
            Lines           : 1,128,024
            Avg. length     : 87.32
            Max. length     : 4,173
            Non-Ascii lines : 6.35 %
    Measuring methods... please be patient...
    Regex           Avg: 0.2885s    Min: 0.2559s    Max: 0.3619s       3,910,404 strings/sec
    Branchy1        Avg: 0.0163s    Min: 0.0158s    Max: 0.0173s      69,345,382 strings/sec
    Branchy2        Avg: 0.0150s    Min: 0.0141s    Max: 0.0168s      75,191,574 strings/sec
    Branchless      Avg: 0.0160s    Min: 0.0156s    Max: 0.0164s      70,479,739 strings/sec
    Hybrid          Avg: 0.0141s    Min: 0.0134s    Max: 0.0151s      80,107,376 strings/sec