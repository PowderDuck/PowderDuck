import sys;
import math;

numberWords = [
    "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", 
    "sixteen", "seventeen", "eighteen", "nineteen", "twenty", "thirty", "fourty", "fifty", "sixty", "seventy", "eighty", "ninety"
];

scales = [
    (0, ""), (2, "hundred"), (3, "thousand"), (6, "million"), (9, "billion"), (12, "trillion")
];

def GetScaleIndex(digits : int) -> int : 
    for scaleIndex in range(len(scales) - 1) : 
        if(scales[scaleIndex][0] <= digits - 1 and scales[scaleIndex + 1][0] > digits - 1) : 
            return scaleIndex;
    return len(scales) - 1;

def Num2Str(number : int) -> str : 
    if(number < 20) : 
        return numberWords[number];

    localIndex = math.floor(number / 10) - 2;
    remainderIndex = number % 10;
    return f"{numberWords[20 + localIndex]} {numberWords[remainderIndex]}";

def Stringify(number : int) -> str : 
    conversion = "";
    digits = math.floor(math.log(number, 10)) + 1;
    startIndex = GetScaleIndex(digits);

    for scaleIndex in range(startIndex + 1) : 
        scaleIndex = startIndex - scaleIndex;
        multiplier = math.pow(10, scales[scaleIndex][0]);
        currentValue = math.floor(number / multiplier);
        if(currentValue <= 0) : 
            continue;
        if(scaleIndex <= 0) : 
            conversion += f"{Num2Str(currentValue)} ";
            break;
        numberName = f"{Num2Str(currentValue)} " if currentValue < 100 else Stringify(currentValue);
        conversion += f"{numberName}{scales[scaleIndex][1]} ";
        number -= currentValue * multiplier;
    return conversion;

print(Stringify(int(sys.argv[1])));