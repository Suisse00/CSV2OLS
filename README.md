# CSV2OLS
A c# utility to convert decimal CSV values to an [OLS](https://github.com/jawi/ols/wiki/OLS-data-file-format) format.

It does :
* Convert decimal values from the CSV to binary where binary high (true, 1) is any value **HIGHER THAN** the *--highsignal* parameter
* Use specific CSV columns (channels) into the output

## Usage
- -f or --frequency \<decimal frequency>[unit]  
  *Default: 100MHz*  
  Specify the frequency of the data sampling from the CSV in order to have the right timing in your OLS client

  where unit could be :
    * Hz (default if omited)
    * KHz
    * MHz
    * GHz
    
  eg. 1 = 1Hz  
  2.3MHz

- -c or --channels  
  *Default: 1*  
  List of columns within the CSV to use as channels into OLS output starting from 1 and delimited with by a comma (eg. 1,3 mean column 1 and 3)
  
  *See -d or --delimiter to set the channels delimiter*

- -hs or --highsignal   
  *Default: 0.1*  
  Decimal value use to convert the CSV decimal value to binary. A decimal value **HIGHER THAN** this parameter will return a binary high (true, 1) otherwise a binary low (false, 0)

- -d or --delimiter  
  *Default: ,*  
  String use as the delimiter to split a CSV line into columns

- -s or --skipline  
  *Default: 2*  
  Number of lines to skip from the top (0 mean none, 1 the first line will be skip)

Note: Current implementation expect you to feed the content of the CSV into the utility (into stdin).
So you should use it such as : 
> CSV2OLS.exe < input.csv > output.osv

## License
Public domain
