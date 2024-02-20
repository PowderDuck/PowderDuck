import math;
import argparse;

parser = argparse.ArgumentParser("CONVERT DECIMAL TO BINARY");

parser.add_argument("-d", "--decimal", help = "Decimal Number");
#parser.add_argument("-n", "--bytes", help = "Number of Bytes");

arguments = parser.parse_args();

def GetBinary(number : int) : 
    binary = [];
    numberOfBytes = math.floor(math.log(number, 2)) + 1;
    dynamicNumber = number;
    binaryString = "";
    for index in range(numberOfBytes) : 
        currentInt = math.pow(2, (numberOfBytes - 1) - index);
        byteValue = math.floor(dynamicNumber / currentInt);
        binary.append(byteValue);
        dynamicNumber %= currentInt;
        binaryString += str(byteValue);
    return (binary[::-1], binaryString);

binArray, binString = GetBinary(int(arguments.decimal));
print(binArray);
print(binString);