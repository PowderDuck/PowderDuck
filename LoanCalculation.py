import argparse;

query = "SELECT * FROM Products WHERE Purchases = (SELECT MAX(Purchases) FROM Products);";

parser = argparse.ArgumentParser("* Loan Calculator *");

parser.add_argument("-a", "--amount", help = "Loan Amount");
parser.add_argument("-n", "--months", help = "Number of Months");
parser.add_argument("-r", "--interest_rate", help = "Interest Rate (in percentages)");

arguments = parser.parse_args();

loanAmount = float(arguments.amount);
months = int(arguments.months);
interest = float(arguments.interest_rate);

def GetMonthlyPayment(amount : float, months : int, interestRate : float) -> float : 
    monthlyRate = (interestRate / 100) / months;
    numerator = amount * monthlyRate;
    denominator = 1 - (1 / pow(1 + monthlyRate, months));
    return numerator / denominator;

monthlyPayment = GetMonthlyPayment(loanAmount, months, interest);
remainingAmount = loanAmount;

print("| Month | Monthly Payment | Decrement | Interest Decrement | Remainder |");

for it in range(months) : 
    interestIncrement = (remainingAmount / months) * (interest / 100);
    decrement = monthlyPayment - interestIncrement;
    remainingAmount -= decrement;
    print("| %i | %.2f | %.2f | %.2f | %.2f |" % (it, monthlyPayment, decrement, interestIncrement, remainingAmount));
    #print("Month : %i | Monthly Payment : %.2f | InterestIncrement : %.2f | Decrement : %.2f | RemainingAmount : %.2f |" % (it + 1, monthlyPayment, interestIncrement, decrement, remainingAmount));
