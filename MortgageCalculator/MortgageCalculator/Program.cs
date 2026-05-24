using System;
using System.Collections.Generic;

public class MortgageCalculator
{
    // Method to calculate the monthly repayment amount
    public static double CalculateMonthlyRepayment(double loanAmount, double annualInterestRate, int loanTermYears)
    {
        // Validate inputs (Edge Cases)
        if (annualInterestRate == 0 || loanTermYears == 0) //Making sure the input for annualInterstRate and loanTermYears is not 0
        {
            throw new ArgumentException("Interest rate or loan term cannot be zero.");
        }

        // Calculate monthly interest rate
        double monthlyInterestRate = annualInterestRate / 12 / 100;
        // Calculate total number of payments
        int numberOfPayments = loanTermYears * 12;
        // Calculate monthly repayment using formula for mortgage payment
        double monthlyRepayment = loanAmount * monthlyInterestRate / (1 - Math.Pow(1 + monthlyInterestRate, -numberOfPayments));
        return monthlyRepayment;
    }

    // Method to calculate the total amount of interest paid over the life of the loan
    public static double CalculateTotalInterestPaid(double loanAmount, double annualInterestRate, int loanTermYears)
    {
        // Validate inputs (Edge Cases)
        if (annualInterestRate == 0 || loanTermYears == 0) //Making sure the input is not 0
        {
            throw new ArgumentException("Interest rate or loan term cannot be zero.");
        }

        // Calculate monthly repayment
        double monthlyRepayment = CalculateMonthlyRepayment(loanAmount, annualInterestRate, loanTermYears);
        // Calculate total amount paid
        int numberOfPayments = loanTermYears * 12;
        double totalAmountPaid = monthlyRepayment * numberOfPayments;
        // Calculate total interest paid
        double totalInterestPaid = totalAmountPaid - loanAmount;
        return totalInterestPaid;
    }

    // Method to calculate the total amount paid over the life of the loan
    public static double CalculateTotalAmountPaid(double loanAmount, double annualInterestRate, int loanTermYears)
    {
        // Validate inputs (Edge Cases)
        if (annualInterestRate == 0 || loanTermYears == 0) //Input cannot be zero
        {
            throw new ArgumentException("Interest rate or loan term cannot be zero.");
        }

        // Calculate monthly repayment
        double monthlyRepayment = CalculateMonthlyRepayment(loanAmount, annualInterestRate, loanTermYears);
        // Calculate total amount paid
        int numberOfPayments = loanTermYears * 12;
        double totalAmountPaid = monthlyRepayment * numberOfPayments;
        return totalAmountPaid;
    }

    // Method to generate the amortization schedule
    public static List<AmortizationScheduleEntry> GenerateAmortizationSchedule(double loanAmount, double annualInterestRate, int loanTermYears)
    {
        // Validate inputs (Ege Cases)
        if (annualInterestRate == 0 || loanTermYears == 0) //Preventing user from using zero
        {
            throw new ArgumentException("Interest rate or loan term cannot be zero.");
        }

        // Initialize schedule list
        List<AmortizationScheduleEntry> schedule = new List<AmortizationScheduleEntry>();
        // Calculate monthly repayment
        double monthlyRepayment = CalculateMonthlyRepayment(loanAmount, annualInterestRate, loanTermYears);
        // Initialize remaining balance
        double remainingBalance = loanAmount;
        // Calculate monthly interest rate
        double monthlyInterestRate = annualInterestRate / 12 / 100;

        // Generate schedule for each month
        for (int month = 1; month <= loanTermYears * 12; month++)
        {
            // Calculate interest payment for the month
            double interestPayment = remainingBalance * monthlyInterestRate;
            // Calculate principal payment for the month
            double principalPayment = monthlyRepayment - interestPayment;
            // Update remaining balance
            remainingBalance -= principalPayment;

            // Create an entry for the amortization schedule
            AmortizationScheduleEntry entry = new AmortizationScheduleEntry(month, monthlyRepayment, interestPayment, principalPayment, remainingBalance);
            schedule.Add(entry);
        }

        return schedule;
    }
}

// Class to represent an entry in the amortization schedule
public class AmortizationScheduleEntry
{
    // Properties
    public int PaymentNumber { get; set; }
    public double PaymentAmount { get; set; }
    public double InterestPaid { get; set; }
    public double PrincipalPaid { get; set; }
    public double RemainingBalance { get; set; }

    // Constructor to initialize the entry
    public AmortizationScheduleEntry(int paymentNumber, double paymentAmount, double interestPaid, double principalPaid, double remainingBalance)
    {
        PaymentNumber = paymentNumber;
        PaymentAmount = paymentAmount;
        InterestPaid = interestPaid;
        PrincipalPaid = principalPaid;
        RemainingBalance = remainingBalance;
    }
}

// Main program
class Program
{
    static void Main(string[] args)
    {
        // Welcome message
        Console.WriteLine("Welcome to the Mortgage Calculator!");
        Console.WriteLine("Please enter the following details:");

        // Prompt user for input
        Console.Write("Loan Amount: ");
        double loanAmount = Convert.ToDouble(Console.ReadLine());

        Console.Write("Annual Interest Rate (%): ");
        double annualInterestRate = Convert.ToDouble(Console.ReadLine());

        Console.Write("Loan Term (in years): ");
        int loanTermYears = Convert.ToInt32(Console.ReadLine());

        try
        {
            // Calculate mortgage details
            double monthlyRepayment = MortgageCalculator.CalculateMonthlyRepayment(loanAmount, annualInterestRate, loanTermYears);
            double totalInterestPaid = MortgageCalculator.CalculateTotalInterestPaid(loanAmount, annualInterestRate, loanTermYears);
            double totalAmountPaid = MortgageCalculator.CalculateTotalAmountPaid(loanAmount, annualInterestRate, loanTermYears);
            List<AmortizationScheduleEntry> schedule = MortgageCalculator.GenerateAmortizationSchedule(loanAmount, annualInterestRate, loanTermYears);

            // Output results to console
            Console.WriteLine("\nSummary:");
            Console.WriteLine("Monthly Repayment: " + monthlyRepayment.ToString("C2"));
            Console.WriteLine("Total Interest Paid: " + totalInterestPaid.ToString("C2"));
            Console.WriteLine("Total Amount Paid: " + totalAmountPaid.ToString("C2"));

            Console.WriteLine("\nAmortization Schedule:");
            Console.WriteLine("{0,-10} {1,-15} {2,-15} {3,-15} {4,-15}", "Payment#", "PaymentAmount", "InterestPaid", "PrincipalPaid", "RemainingBalance");
            foreach (var entry in schedule)
            {
                Console.WriteLine("{0,-10} {1,-15} {2,-15} {3,-15} {4,-15}", entry.PaymentNumber, entry.PaymentAmount.ToString("C2"), entry.InterestPaid.ToString("C2"), entry.PrincipalPaid.ToString("C2"), entry.RemainingBalance.ToString("C2"));
            }
        }
        catch (ArgumentException ex)
        {
            // Handle exception if inputs are invalid
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}
