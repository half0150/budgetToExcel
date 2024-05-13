using Microsoft.AspNetCore.Mvc.RazorPages;

namespace budgetToExcel.Pages
{
    public class graph_allocationModel : PageModel
    {
        public decimal FoodAllocation { get; set; }
        public decimal EntertainmentAllocation { get; set; }
        public decimal SavingsAllocation { get; set; }
        public decimal OtherExpensesAllocation { get; set; }

        public void OnGet(decimal foodAllocation, decimal entertainmentAllocation, decimal savingsAllocation, decimal otherExpensesAllocation)
        {
            FoodAllocation = decimal.Parse(Request.Query["FoodAllocation"]);
            EntertainmentAllocation = decimal.Parse(Request.Query["EntertainmentAllocation"]);
            SavingsAllocation = decimal.Parse(Request.Query["SavingsAllocation"]);
            OtherExpensesAllocation = decimal.Parse(Request.Query["OtherExpensesAllocation"]);
        }
    }
}
