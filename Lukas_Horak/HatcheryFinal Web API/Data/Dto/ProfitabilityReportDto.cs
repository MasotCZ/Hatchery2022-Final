namespace HatcheryFinal_Web_API.Data.Dto
{
    /// <summary>
    /// Dto for profitability report.
    /// </summary>
    public class ProfitabilityReportDto
    {
        /// <summary>
        /// Partner that is in the report.
        /// </summary>
        public CreditPartnerFullInfoDto Partner { get; set; }
        
        /// <summary>
        /// Total credit from all accepted requests.
        /// </summary>
        public decimal TotalCredit { get; set; }

        /// <summary>
        /// The success rate of contacts.
        /// </summary>
        public decimal SuccessRate { get; set; }
    }
}
