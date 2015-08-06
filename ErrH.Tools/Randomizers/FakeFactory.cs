using System;
using System.Text;
using ErrH.Tools.Extensions;

namespace ErrH.Tools.Randomizers
{
    public class FakeFactory
    {
        //private static int _nextSeed = (int)System.DateTime.Now.Ticks;

        private Random _random;
        private string[] _vowels;
        private string[] _consonants;
        private string[] _companySuffixes;

        public FakeFactory()
        {
            //_random = (seed == -1) ? new Random() : new Random(seed);
            /*if (seed == -1)
            {
                _random = new Random(_nextSeed);
                _nextSeed++;
            }
            else
            {
                _random = new Random(seed);
            }*/

            _random = ThreadSafe.LocalRandom;

            _vowels = new string[] { "a", "e", "i", "o", "u" };

            _consonants = new string[] { "b",
                                        "c",
                                        "d",
                                        "f",
                                        "g",
                                        "h",
                                        "j",
                                        "k",
                                        "l",
                                        "m",
                                        "n",
                                        "p",
                                        "qu",
                                        "r",
                                        "s",
                                        "t",
                                        "v",
                                        "w",
                                        "x",
                                        "y",
                                        "z"
        };
            _companySuffixes = new string[] { "Inc.", "Ltd.", "Co." };

        }

        public string Company
        {
            get
            {
                return this.ProperNoun + " " + this.CompanySuffix;
            }
        }

        public string CompanySuffix
        {
            get { return _companySuffixes[_random.Next(_companySuffixes.Length - 1)]; }
        }

        public string PrintCompany
        {
            get
            {
                string propr = this.ProperNoun;
                if (this.Truthy) propr += " " + this.ProperNoun;

                var suffixes = new string[] {
            "Arts & Graphics Services",
            "Documents Solutions Inc.",
            "Graphics and Arts Printing Services",
            "Print and Design Studio",
            "Printing",
            "Printing Co., Inc.",
            "Printing Company, Inc.",
            "Printing Press",
            "Printing Services, Inc.",
            "Prints",
        };

                return propr + " " + suffixes.RandomItem();
            }
        }

        public string Vowel
        {
            get { return _vowels[_random.Next(_vowels.Length - 1)]; }
        }
        private string V { get { return this.Vowel; } }
        private string C { get { return this.Consonant; } }

        public string Consonant
        {
            get { return _consonants[_random.Next(_consonants.Length - 1)]; }
        }

        public string Syllable
        {
            get
            {
                switch (_random.Next(0, 14))
                {
                    case 0: return C + V;
                    case 1: return C + V;
                    case 2: return C + V;
                    case 3: return C + V;
                    case 4: return C + V;
                    case 5: return C + V + C;
                    case 6: return C + V + C;
                    case 7: return C + V + C;
                    case 8: return C + V + C;
                    case 9: return C + V + C;
                    case 10: return V + C;
                    case 11: return V + C;
                    case 12: return V + C;
                    case 13: return V;
                    case 14: return C;
                }
                return "";
            }
        }



        /// <summary>
        /// 1 fake word in title case
        /// </summary>
        public string ProperNoun
        {
            get { return this.Word.ToTitleCase(); }
        }

        public string Word
        {
            get
            {
                var str = this.Syllable + this.Syllable;
                if (this.Truthy) str += this.Syllable;
                return str;
            }
        }


        /// <summary>
        /// 3 fake words
        /// </summary>
        public string Text
        {
            get { return this.Word + " " + this.Word + " " + this.Word; }
        }

        public string Email
        {
            get { return this.Word + "@" + this.Word + "." + this.Syllable; }
        }

        public string Street
        {
            get { return "#" + this.Int(1, 999) + " " + this.ProperNoun + " St."; }
        }


        public string School
        {
            get
            {
                string propr = this.ProperNoun + " " + this.ProperNoun;
                if (this.Truthy)
                    propr += " " + this.ProperNoun;
                else
                    if (this.Chance(1, 4)) propr = "St. " + propr;

                string[] suffixes = { "Academy"
                            , "Montesorri School"
                            , "Parochial School"
                            , "University"
                            , "High School"
                            , "Elementary School"
                            , "College" };

                return propr + " " + suffixes.RandomItem();
            }
        }


        /*public int Id
        {
            get { return _random.Next(1, 999); }
        }*/

        public bool Truthy
        {
            get { return this.Int(0, 1) == 1; }
        }

        public bool Chance(int numerator, int denominator)
        {
            return this.Int(1, denominator) == numerator;
        }

        public string Phone
        {
            get
            {
                var area = _random.Next(2, 99);
                var part1 = _random.Next(100, 999);
                var part2 = _random.Next(1000, 9999);
                return "({0}) {1}-{2}".f(area, part1, part2);
            }
        }

        public byte[] Bytes
        {
            get
            {
                return Encoding.UTF8.GetBytes(this.Word);
            }
        }

        public DateTime BirthDate
        {
            get
            {
                int yrNow = DateTime.Today.Year;
                var minDate = new DateTime(yrNow - 65, 1, 1);
                var maxDate = new DateTime(yrNow - 25, 1, 1);

                int dayRange = (maxDate - minDate).Days;

                return minDate.AddDays(_random.Next(dayRange));
            }
        }

        public int Int(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue + 1);
        }


        /// <summary>
        /// Returns random number between 0.0 and 1.0.
        /// For Percentage, Rate and Share Percentage and etc.
        /// </summary>
        public decimal Decimal
        {
            get
            {
                return Convert.ToDecimal(this.Double);
            }
        }

        public double Double
        {
            get
            {
                return _random.NextDouble();
            }
        }

        //public decimal Money { get 
        //{ return this.Money(1, 1000000); }}

        public decimal Money(int min, int max)
        {
            var wholeNum = this.Int(min, max);
            return Math.Round(wholeNum + this.Decimal, 2);
        }


        public DateTime Date(int minYr, int maxYr)
        {
            var minDate = new DateTime(minYr, 1, 1);
            var maxDate = new DateTime(maxYr, 1, 1);

            int dayRange = (maxDate - minDate).Days;

            return minDate.AddDays(_random.Next(dayRange));
        }



        public T Pick1<T>(params T[] args)
        {
            return args.RandomItem();
        }


    }

}
