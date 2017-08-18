using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyCalculator
{
    class Calc
    {
        static bool TestValidOperands(List<String> s)
        {
            var operands = new[] {"*","/","+","-"};
            if (!operands.Any(s.Contains))
                return false;
            return true;
        }

        static bool TestInputFormat(List<String> s)
        {
            double x;
            if (!(s[0] == "(" || double.TryParse(s[0], out x)))
                return false;
            for (int i = 1; i < s.Count(); i++)
            {
                if (i % 2 == 0)
                {
                    
                    if (!double.TryParse(s[i], out x))
                    {
                        return false;
                    }
                }

                if (i == s.Count()-1)
                {
                     if (!double.TryParse(s[i], out x))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        static double Multiply(double a, double b)
        {
            return a * b;
        }

        static double Divide(double a, double b)
        {
            return a / b;
        }

        static double Add(double a, double b)
        {
            return a + b;
        }

        static double Subtract(double a, double b)
        {
            return a - b;
        }

        static void ParseOutCalculation(List<String> s, int level)
            //level 1 = multiplication and division
            //level 2 = addition and subtraction
        {
            double value = 0;
            if (level == 1)
            {
                var operands = new[] { "*", "/"};
                for (int i = 0; i < s.Count(); i++)
                {
                    if (operands.Any(s[i].Contains))
                    {
                        i--;
                        switch (s[i + 1])
                        {
                            case "*":
                                value = Multiply(Convert.ToDouble(s[i]), Convert.ToDouble(s[i + 2]));
                                break;
                            case "/":
                                if (Convert.ToDouble(s[i + 2]) != 0)
                                {
                                    value = Divide(Convert.ToDouble(s[i]), Convert.ToDouble(s[i + 2]));
                                    break;
                                }
                                else
                                {
                                    System.Console.WriteLine("This calculation contains an invalid divide by 0.  Process terminated.");
                                    Environment.Exit(0);
                                    break;
                                }
                        }
                        s[i] = value.ToString();
                        for (int j = i + 2; j > i; j--)
                            s.RemoveAt(j);

                    }
                }
            }
            else if (level == 2)
            {
                var operands = new[] { "+", "-" };
                int x = 0;
                while (s.Count() > 1)
                {
                    if (operands.Any(s[x].Contains))
                    {
                        x--;
                        switch (s[x + 1])
                        {
                            case "+":
                                value = Add(Convert.ToDouble(s[x]), Convert.ToDouble(s[x + 2]));
                                break;
                            case "-":
                                value = Subtract(Convert.ToDouble(s[x]), Convert.ToDouble(s[x + 2]));
                                break;
                        }
                        s[x] = value.ToString();
                        for (int j = x + 2; j > x; j--)
                            s.RemoveAt(j);
                    }
                    x++;
                }
            }
        }

        static void ReportError(string s)
        {
            System.Console.WriteLine(s);
            System.Console.WriteLine("Use '*' for multiplication, '/' for division, '+' for addition and '-' for subtraction.");
            System.Console.WriteLine("The application follows the standard order of operation which is left to right, multiplication and division first, addition and subtraction last.");
        }

        static void Main(string[] args)
        {

            if (args.Length == 0)
            {
                ReportError("The application is expecting a formula to calculate.");
            }
            else
            {
                string argString = String.Join("", args);
                if (argString.Contains("(") || argString.Contains(")"))
                {
                    ReportError("The current version of the calculator doesn't support parenthesis to dictate order of operation.");
                }
                else
                {
                    Regex re = new Regex(@"([\+\-\*\^\/\ ])");
                    List<String> formattedArgs = re.Split(argString).Select(t => t.Trim()).Where(t => t != "").ToList();

                    if (!TestValidOperands(formattedArgs))
                    {
                        ReportError("Your calculation must contain at least 1 operand.");
                    }

                    if (!TestInputFormat(formattedArgs))
                    {
                        ReportError("Your formula is in the wrong format. It must start and end with a valid number and each number must be separated by a valid operand.");
                    }
                    else
                    {
                        try
                        {
                            ParseOutCalculation(formattedArgs, 1);
                            ParseOutCalculation(formattedArgs, 2);
                            System.Console.WriteLine(argString + " = " + formattedArgs[0]);
                        }
                        catch
                        {
                            System.Console.WriteLine("Unexpected Error Encountered.");
                        }
                    }

                }
            }

        }
    }
}
