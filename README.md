# ASCII-Benchmark
Inspired by https://lemire.me/blog/2020/07/21/avoid-character-by-character-processing-when-performance-matters/

Test strings in the files `allCountries.txt.gz` and `cities500.txt.gz` are [sourced from Geonames.org](https://download.geonames.org/export/dump/) where I have extracted the second column (`name`) of the corresponding files. The `enwik8.gz` file is sourced from [The Large Text Compression Benchmark](http://mattmahoney.net/dc/textdata.html). Testfiles have been gzipped in order to save space. These files have a nice mix of ASCII / non-ASCII data.

# Results (Intel Core i9-10900X):

    Benching allCountries.txt.gz
            Lines           : 7,522,986
            Avg. length     : 14.58
            Max. length     : 151
            Non-Ascii lines : 23.52 %%
    Measuring methods... please be patient...
    Regex           Avg: 1.1114s    Min: 1.0492s    Max: 1.1788s       6,769,036 strings/sec
    Branchy1        Avg: 0.0522s    Min: 0.0495s    Max: 0.0540s     144,076,856 strings/sec
    Branchy2        Avg: 0.0577s    Min: 0.0504s    Max: 0.0623s     130,480,608 strings/sec
    Branchless      Avg: 0.0573s    Min: 0.0528s    Max: 0.0616s     131,280,711 strings/sec
    Hybrid          Avg: 0.0594s    Min: 0.0504s    Max: 0.0903s     126,719,761 strings/sec

    Benching cities500.txt.gz
            Lines           : 165,957
            Avg. length     : 10.14
            Max. length     : 65
            Non-Ascii lines : 20.12 %%
    Measuring methods... please be patient...
    Regex           Avg: 0.0196s    Min: 0.0192s    Max: 0.0201s       8,453,229 strings/sec
    Branchy1        Avg: 0.0012s    Min: 0.0009s    Max: 0.0015s     143,309,759 strings/sec
    Branchy2        Avg: 0.0011s    Min: 0.0009s    Max: 0.0013s     146,109,487 strings/sec
    Branchless      Avg: 0.0012s    Min: 0.0009s    Max: 0.0014s     141,202,747 strings/sec
    Hybrid          Avg: 0.0011s    Min: 0.0009s    Max: 0.0014s     147,760,317 strings/sec

    Benching enwik8.gz
            Lines           : 1,128,024
            Avg. length     : 87.32
            Max. length     : 4,173
            Non-Ascii lines : 6.35 %%
    Measuring methods... please be patient...
    Regex           Avg: 0.2519s    Min: 0.2158s    Max: 0.2809s       4,478,492 strings/sec
    Branchy1        Avg: 0.0157s    Min: 0.0142s    Max: 0.0172s      71,839,465 strings/sec
    Branchy2        Avg: 0.0151s    Min: 0.0144s    Max: 0.0165s      74,767,351 strings/sec
    Branchless      Avg: 0.0149s    Min: 0.0141s    Max: 0.0156s      75,526,212 strings/sec
    Hybrid          Avg: 0.0147s    Min: 0.0143s    Max: 0.0155s      76,516,251 strings/sec
