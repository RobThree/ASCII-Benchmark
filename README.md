# Ascii-Benchmark
Inspired by https://lemire.me/blog/2020/07/21/avoid-character-by-character-processing-when-performance-matters/

Test strings are [sourced from Geonames.org](https://download.geonames.org/export/dump/) where I have extracted the second column (`name`) of the corresponding files and have been gzipped in order to save space. These files have a nice mix of ASCII / non-ASCII data.