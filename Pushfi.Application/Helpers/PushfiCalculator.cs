namespace Pushfi.Application.Helpers
{
    public static class PushfiCalculator
    {
        private const double lowOfferRate = 0.75;
        private const double highOfferRate = 1.25;
        private const int numberOfPeriods = 48;
        private const int lowOfferRoundFactor = 5000;
        private const int highOfferRoundFactor = 10000;
        private const int lowRevolvingLine = 15000;
        private const int highRevolvingLine = 45000;

        private const double TierOneRateFrom = 5.99;
        private const double TierOneRateTo = 15.99;

        private const double TierTwoRateFrom = 8.49;
        private const double TierTwoRateTo = 18.79;

        private const double TierThreeRateFrom = 10.99;
        private const double TierThreeRateTo = 22.99;

        private const double TierFourRateFrom = 16.99;
        private const double TierFourRateTo = 33.99;

        // Returns list with low and high offers
        public static List<int> CalculateOffers(List<int> termLoans)
        {
            var lowOffer = termLoans[0] + lowRevolvingLine;
            var highOffer = termLoans[1] + highRevolvingLine;

            var result = new List<int>()
            {
                lowOffer,
                highOffer
            };

            return result;
        }

        // Returns list with low and high term loans
        public static List<int> CalculateTermLoans(decimal monthlyIncome, decimal totalMonthlyPayments)
        {
            var termLoanProjection = TermLoanProjection(monthlyIncome, totalMonthlyPayments);

            // Round to nearest factor
            var lowTermLoan = (int)Math.Round(termLoanProjection * (decimal)lowOfferRate / lowOfferRoundFactor) * lowOfferRoundFactor;
            var highTermLoan = (int)Math.Round(termLoanProjection * (decimal)highOfferRate / highOfferRoundFactor) * highOfferRoundFactor;

            var result = new List<int>()
            {
                lowTermLoan,
                highTermLoan
            };

            return result;
        }

        private static decimal TermLoanProjection(decimal monthlyIncome, decimal totalMonthlyPayments)
        {
            var oneThirdExcessMonthlyIncome = OneThirdExcessMonthlyIncome(monthlyIncome, totalMonthlyPayments);
            return oneThirdExcessMonthlyIncome * numberOfPeriods;
        }

        private static decimal OneThirdExcessMonthlyIncome(decimal monthlyIncome, decimal totalMonthlyPayments)
        {
            return (monthlyIncome - totalMonthlyPayments) / 3;
        }

        // TODO: return dictionary with type and values
        public static List<double> CalculateTier(int creditScore)
        {
            if (creditScore >= 640 && creditScore <= 659)
            {
                // Tier 4
                return new List<double>()
                {
                    TierFourRateFrom,
                    TierFourRateTo
                };
            }
            else if (creditScore >= 660 && creditScore <= 689)
            {
                // Tier 3
                return new List<double>()
                {
                    TierThreeRateFrom,
                    TierThreeRateTo
                };
            }
            else if (creditScore >= 690 && creditScore <= 719)
            {
                // Tier 2
                return new List<double>()
                {
                    TierTwoRateFrom,
                    TierTwoRateTo
                };
            }
            else if (creditScore >= 720 && creditScore <= 900)
            {
                // Tier 1
                return new List<double>()
                {
                    TierOneRateFrom,
                    TierOneRateTo
                };
            }
            else if (creditScore >= 500 && creditScore <= 639)
            {
                // DECLINE
                return new List<double>();
            }
            else
            {
                return new List<double>();
            }
        }
    }
}

// total funding achieved will be dynamicaly (the last from the email) - add it as field in broker form.
// Each broker can have different value. In future brokers should be possible to update the value
