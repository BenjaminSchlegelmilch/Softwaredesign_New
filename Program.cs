using System;

namespace L04
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter Value from 0 to 1023");
            string numberString = Console.ReadLine();
            Console.WriteLine("Enter Number Base");
            string fromBaseString = Console.ReadLine();
            Console.WriteLine ("Enter convert Number Base");
            string toBaseString = Console.ReadLine();
            int fromBase = Int32.Parse(fromBaseString);
            int toBase = Int32.Parse(toBaseString);
            int number = Int32.Parse(numberString);

            if (0 <= number && number <= 1023 && 2<= fromBase && fromBase <=10){
                ConvertToBaseFromDecimal(number, fromBase);
                ConvertToDecimalFromBase(number, fromBase);
            }
            if (2 <= toBase && toBase <= 10){
                ConvertNumberToBaseFromBase(number, fromBase, toBase);
            }
            else {
            Console.WriteLine("Value must be between 0 and 1023 and base numbers must be between 2 and 10");
            }
        }
        
       static void ConvertToBaseFromDecimal(int number, int fromBase) {
            int lastDigit = (number % 10)*(fromBase^0);
            int secondlastDigit = ((number % 100 - lastDigit)/10)*(fromBase^1);
            int thirtlastDigit = ((number % 1000 - lastDigit - secondlastDigit)/100)*(fromBase^2);
            int fourthlastDigit = ((number % 10000 - lastDigit - secondlastDigit - thirtlastDigit)/1000)*(fromBase^3);
            int FinalNumber=(lastDigit + secondlastDigit + thirtlastDigit + fourthlastDigit);
            Console.WriteLine("The Hexal number of " + number +" is " + FinalNumber);
       }
  
        static void ConvertToDecimalFromBase(int number, int fromBase){
            int firstDezimalNumber = (number % fromBase); 
            int firstDevisionNumber = (number / fromBase);   
            int secondDezimalNumber = (firstDevisionNumber % fromBase); 
            int secondDevisionNumber = (firstDevisionNumber / fromBase);
            int thirtDezimalNumber = (secondDevisionNumber % fromBase); 
            int thirtDevisionNumber = (secondDevisionNumber / fromBase);
            int fourthDezimalNumber = (thirtDevisionNumber % fromBase); 
            int fourthDevisionNumber = (thirtDevisionNumber  / fromBase);
            string finalDezimalNumberString = (firstDezimalNumber +""+  secondDezimalNumber +""+ thirtDezimalNumber +""+ fourthDezimalNumber);
            int finalDezimalNumberInt = Int32.Parse(finalDezimalNumberString);
            Console.WriteLine("The Dezimal Number of the Hexalnumber " + number + " is " + finalDezimalNumberInt);
        }
        static void ConvertNumberToBaseFromBase(int number, int fromBase, int endNumberBaseInt, int FinalNumber){
            // Habe leider nicht verstanden, wie ich diese Funktion schreiben kann.
        }
    }
}
