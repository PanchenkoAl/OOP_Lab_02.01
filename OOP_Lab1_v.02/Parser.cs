using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_Lab1_v._02
{
    internal class Parser
    {
        public string ParseExpression(string exp, Cell[,] table)
        {
            exp = exp.Replace(" ", "");
            if(exp == null)
            {
                MessageBox.Show("#ERROR");
                return "0";
            }
            if (exp[0] != '(')
            {
                string temp = exp;
                exp = null;
                exp += "(";
                exp += temp;
            }
            if (exp[exp.Length - 1] != ')')
                exp += ")";

            exp = FindIncDec(exp);
            exp = exp.Replace(" ", "");
            exp = SetBrackets(exp);
            //MessageBox.Show("SB " + exp);
            exp = EvaluateBrackets(exp, table);
            return exp;
        }

        public bool IsNumber(char c)
        {
            if ("1234567890".IndexOf(c) != -1)
                return true;
            else
                return false;
        }

        public Lexer InstanceOfLexer = new Lexer();

        public string FindIncDec(string exp)
        {
            for(int i = 0; i < exp.Length - 2; i ++)
            {
                if(exp[i] == 'i' && exp[i+1] == 'n' && exp[i + 2] == 'c')
                {
                    string t1 = null;
                    string t2 = null;
                    for (int k = 0; k < i; k++)
                        t1 += exp[k];
                    for (int k = i + 3; k < exp.Length; k++)
                        t2 += exp[k];
                    t1 += "1+";
                    t1 += t2;
                    exp = t1;
                    return exp;
                }
            }
            for (int i = 0; i < exp.Length - 2; i++)
            {
                if (exp[i] == 'd' && exp[i + 1] == 'e' && exp[i + 2] == 'c')
                {
                    string t1 = null;
                    string t2 = null;
                    for (int k = 0; k < i; k++)
                        t1 += exp[k];
                    for (int k = i + 3; k < exp.Length; k++)
                        t2 += exp[k];
                    t1 += "-1+";
                    t1 += t2;
                    exp = t1;
                    return exp;
                }
            }
            return exp;
        }

        public string SetBrackets(string exp)
        {
            for (int i = 0; i < exp.Length; i++)
            {
                if (exp[i] == '^')
                {
                    for (int k = i + 1; k < exp.Length; k++)
                    {
                        if (exp[k] == '(')
                        {
                            while (exp[k] != ')' && k < exp.Length)
                                k++;
                        }
                        if (InstanceOfLexer.IsDelim(exp[k]) || k == exp.Length - 1)
                        {
                            exp = exp.Insert(k, ")");
                            break;
                        }
                    }
                    for (int k = i - 1; k > 0; k--)
                    {
                        if (exp[k] == ')')
                        {
                            while (exp[k] != '(' && k > 0)
                                k--;
                        }
                        if (InstanceOfLexer.IsDelim(exp[k]) || k == 1)
                        {
                            exp = exp.Insert(k - 1, "(");
                            i++;
                            break;
                        }
                    }
                }
            }
            for (int i = 0; i < exp.Length; i++)
            {
                if (exp[i] == '*' || exp[i] == '/')
                {
                    for (int k = i + 1; k < exp.Length; k++)
                    {
                        if (exp[k] == '(')
                        {
                            while (exp[k] != ')' && k < exp.Length)
                                k++;
                        }
                        if (InstanceOfLexer.IsDelim(exp[k]) || k == exp.Length - 1)
                        {
                            exp = exp.Insert(k, ")");
                            break;
                        }
                    }
                    for (int k = i - 1; k > 0; k--)
                    {
                        if (exp[k] == ')')
                        {
                            while (exp[k] != '(' && k > 0)
                                k--;
                        }
                        if (InstanceOfLexer.IsDelim(exp[k]) || k == 1)
                        {
                            exp = exp.Insert(k - 1, "(");
                            i++;
                            break;
                        }
                    }
                }
            }
            for (int i = 0; i < exp.Length; i++)
            {
                if (exp[i] == '+' || exp[i] == '-')
                {
                    for (int k = i + 1; k < exp.Length; k++)
                    {
                        if(exp[k] == '(')
                        {
                            while (exp[k] != ')' && k < exp.Length)
                                k++;
                        }
                        if (InstanceOfLexer.IsDelim(exp[k]) || k == exp.Length - 1)
                        {
                            exp = exp.Insert(k, ")");
                            break;
                        }
                    }
                    for (int k = i - 1; k > 0; k--)
                    {
                        if (exp[k] == ')')
                        {
                            while (exp[k] != '(' && k > 0)
                                k--;
                        }
                        if (InstanceOfLexer.IsDelim(exp[k]) || k == 1)
                        {
                            exp = exp.Insert(k - 1, "(");
                            i++;
                            break;
                        }
                    }
                }
            }
            //MessageBox.Show("ret suc");
            return exp;
        }
        public string EvaluateBrackets(string exp, Cell[,] table)
        {
            int sm = 0;
            int em = 0;
            if(exp == null)
            {
                MessageBox.Show("#ERROR");
                return "0";
            }
            //MessageBox.Show("Evaluating brackets started");
            a:
            for (int i = exp.Length - 1; i >= 0; i --)
            {
                //MessageBox.Show("i = " +i + "char = " + exp[i]);
                if (exp[i] == '(')
                {
                    //MessageBox.Show("Char '(' have been found at position: " + i);
                    sm = i;
                    for (int j = 0; j < exp.Length; j++)
                    {
                        if (exp[j] == ')')
                        {
                            //MessageBox.Show("Char ')' have been fount at position: " + j);
                            em = j;
                            string subExp = "";
                            for (int k = sm + 1; k < em; k++)
                            {
                                subExp += exp[k];
                            }
                            //MessageBox.Show("subExp value = " + subExp);
                            subExp = EvaluateExpression(subExp, table);
                            //MessageBox.Show("subExp value avter evaluating = " + subExp);
                            string tempExp = null;
                            for(int o = 0; o < sm; o++)
                            {
                                tempExp += exp[o];
                            }
                            //MessageBox.Show("tempexp = " + tempExp);
                            tempExp += subExp;
                            //MessageBox.Show("tempexp = " + tempExp);
                            for (int o = em + 1; o < exp.Length; o++)
                            {
                                tempExp += exp[o];
                            }
                            //MessageBox.Show("tempexp = " + tempExp);
                            exp = tempExp;
                            //MessageBox.Show("exp = " + exp);
                            goto a;
                        }
                    }
                }
            }
            return exp;
        }

        public string EvaluateExpression(string exp, Cell[,] table)
        {
            int idm;
            if (exp == null)
            {
                MessageBox.Show("#ERROREE");
                return "0";
            }
            string op1 = null;
            string op2 = null;
            double res1 = 0, res2;
            char delim = '0';
            bool flag = false;

            for(int i = 0; i < exp.Length; i ++)
            {          
                int rb = exp.Length, lb = - 1;
                int delimIdx = FindDelim(exp);
                delim = exp[delimIdx];
                int cou = 0;
                for(int oo = 0; oo < exp.Length; oo ++)
                {
                    if (InstanceOfLexer.IsDelim(exp[oo]))
                    {
                        cou++;
                    }
                }
                if(cou == 0)
                {
                    flag = true;
                }
                for(int k = delimIdx + 1; k < exp.Length; k ++)
                {
                    if (InstanceOfLexer.IsDelim(exp[k]))
                    {
                        rb = k;
                    }
                }
                for (int k = delimIdx - 1; k > 0; k--)
                {
                    if (InstanceOfLexer.IsDelim(exp[k]))
                    {
                        lb = k;
                    }
                }
                for(int k = lb + 1; k < delimIdx; k ++)
                {
                    op1 += exp[k];
                }
                for (int k = delimIdx + 1; k < rb; k++)
                {
                    op2 += exp[k];
                }
                if(flag)
                {
                    op1 = exp;
                    res1 = FindCell(op1, table);
                    return res1.ToString();
                }
                res1 = FindCell(op1, table);
                res2 = FindCell(op2, table);
                res1 = CalculateResult(res1, res2, delim);
                return res1.ToString();
            }
            return res1.ToString();
        }

        public int FindDelim (string exp)
        {
            int delimIdx = 0;
            for(int i = 0; i < exp.Length; i ++)
            {
                if (exp[i] == '^')
                {
                    delimIdx = i;
                    return delimIdx;
                }
            }
            for (int i = 0; i < exp.Length; i++)
            {
                if (exp[i] == '*' || exp[i] == '/')
                {
                    delimIdx = i;
                    return delimIdx;
                }
            }
            for (int i = 0; i < exp.Length; i++)
            {
                if (exp[i] == '+' || exp[i] == '-')
                {
                    delimIdx = i;
                    return delimIdx;
                }
            }
            return delimIdx;
        }

        public double FindCell(string exp, Cell[,] table)
        {
            double oper1 = 0.0;
            string addressC = null;
            string addressR = null;
            Lexer lexer = new Lexer();
            if (exp == null)
                MessageBox.Show("exp is null");
            if (exp != null)
                if (IsNumber(exp[0]))
                {
                    oper1 = Double.Parse(exp);
                    return oper1;
                }
                else
                {
                    int it = 0;
                    while (it < exp.Length && InstanceOfLexer.IsLetter(exp[it]))
                    {
                        addressC += exp[it];
                        it++;
                    }
                    int itt = it;
                    while (itt < exp.Length && InstanceOfLexer.IsNumber(exp[itt]))
                    {
                        addressR += exp[itt];
                        itt++;
                    }
                    int c1 = 0, c2 = 0;
                    //MessageBox.Show("AddressC = " + addressC + " | " + addressC.Length.ToString());
                    if(addressC != null)
                    for (int k = 0; k < addressC.Length; k++)
                    {
                        if (addressC.Length != 1)
                            c1 += ((int)addressC[k] - 64) * (26 ^ (addressC.Length - k) - 1) - 1;
                        else
                            c1 += ((int)addressC[k] - 65);
                    }
                    if(addressR != null)
                    for (int k = 0; k < addressR.Length; k++)
                    {
                        c2 += ((int)addressR[k] - 48) * (10 ^ (addressR.Length - k - 1));
                    }
                    //MessageBox.Show("c1 = " + c1.ToString() + " c2 = " + c2.ToString());
                    //MessageBox.Show("table cv = " + table[c2, c1].cValue);
                    if(c1 < 100 && c2 < 100)
                    {
                        string buffer = ParseExpression(table[c2, c1].cValue, table);
                        oper1 = Double.Parse(buffer);
                        return oper1;
                    }
                    else
                    {
                        MessageBox.Show("#ERROR");
                        return 0.0;
                    }
                }
            else
                return 0.0;
        }

        public double CalculateResult(double r1, double r2, char del)
        {
            switch(del)
            {
                case '+':
                    return r1 + r2;
                case '-':
                    return r1 - r2;
                case '*':
                    return r1 * r2;
                case '/':
                    return r1 / r2;
                default:
                    MessageBox.Show("Wrong operation");
                    return 0.0;
            }
        }
    }
}
