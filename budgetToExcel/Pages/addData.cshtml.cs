using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeOpenXml;
using System;
using System.IO;

namespace budgetToExcel.Pages
{
    public class addDataModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnPost(int income, int expenses, int phonesubscription, decimal taxRate)
        {
            // fortæller hvor filen skal gemmes og hvad filen skal hedde
            string fileName = "data.xlsx";
            string filePath = Path.Combine(Environment.CurrentDirectory, fileName);

            if (taxRate > 1)
            {
                taxRate = taxRate / 100;
            }

            // beregner hvor meget der skal betales i skat, og hvor meget der til overs osv...
            decimal taxAmount = income * taxRate;
            decimal incomeAfterTax = income - taxAmount;
            decimal difference = incomeAfterTax - expenses - phonesubscription;

            //Budget forslag graf

            decimal foodAllocation = incomeAfterTax * 0.3m;
            decimal entertainmentAllocation = incomeAfterTax * 0.2m;
            decimal savingsAllocation = incomeAfterTax * 0.2m;
            decimal otherExpensesAllocation = incomeAfterTax * 0.3m;



            // indæstter data til excel-ark
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                worksheet.Cells[1, 1].Value = "Indtægter (før skat)";
                worksheet.Cells[1, 2].Value = "Indtægter (efter skat)";
                worksheet.Cells[1, 3].Value = "Udgifter";
                worksheet.Cells[1, 4].Value = "Mobilabonnement";
                worksheet.Cells[1, 5].Value = "Skatteprocent";
                worksheet.Cells[1, 6].Value = "Betalt skat";
                worksheet.Cells[1, 8].Value = "Overskud";

                worksheet.Cells[2, 1].Value = income + ".kr"; 
                worksheet.Cells[2, 2].Value = incomeAfterTax + ".kr"; 
                worksheet.Cells[2, 3].Value = expenses + ".kr";
                worksheet.Cells[2, 4].Value = phonesubscription + ".kr";
                worksheet.Cells[2, 5].Value = taxRate * 100 + "%"; 
                worksheet.Cells[2, 6].Value = taxAmount + ".kr";
                worksheet.Cells[2, 8].Value = difference + ".kr";

                // autosizer kollonner, så det passer til tekstens længde

                for (int col = 1; col <= 8; col++)
                {
                    worksheet.Column(col).AutoFit();
                }

                // gemmer excel filen i den path jeg har sagt den skal, som er current directory i dette tilfælde

                package.SaveAs(new FileInfo(filePath));
            }

            // diagere brugeren til siden (graph_alloocation), hvor at brugeren kan få et budget forslag
            return RedirectToPage("/graph_allocation", new
            {
                FoodAllocation = foodAllocation,
                EntertainmentAllocation = entertainmentAllocation,
                SavingsAllocation = savingsAllocation,
                OtherExpensesAllocation = otherExpensesAllocation,
            });

        }
    }
}
