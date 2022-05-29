using BusinessLogic.DTO_s;
using DataLogic1.DBModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Color = MigraDoc.DocumentObjectModel.Color;

namespace BusinessLogic.Services
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetProductList();
        Task AddProductToCart(ShoppingCartDTO shoppingCartDTO);
        Task<CartDTO> GetCartList();
        Task UpdateProductToCart(ShoppingCartDTO shoppingCartDTO);
        Task DeleteProductFromCart(int cartId);
        FileStreamResult BillGenerate(List<int> cartids);
    }
    public class ProductService : IProductService
    {
        private ShoppingCartContext ShoppingCartContext { get; }
        DateTime dayToday = DateTime.Now;
        Document document = new Document();

        public ProductService(ShoppingCartContext context)
        {
            ShoppingCartContext = context;
        }

        public async Task<List<ProductDTO>> GetProductList()
        {
            try
            {
                var productDTOList = await ShoppingCartContext.TblProducts.Select(x => new ProductDTO
                {
                    ProductId = x.ProductId,
                    ProductcategoryId = x.FkProductcategoryId,
                    ProductName = x.ProductName,
                    ProductCost = x.TblProductCostTables.First(y => y.FkProductId == x.ProductId).ProductCost
                }).ToListAsync();

                var productCatIDs = productDTOList.Select(x => x.ProductId).ToList();
                var ProductcategoryIds = productDTOList.Select(x => x.ProductcategoryId).ToList();

                var deliveryChargesList = await ShoppingCartContext.TblDeliveryCharges.Where(x => ProductcategoryIds.Contains(x.FkProductcategoryId)).ToListAsync();

                var discounts = await ShoppingCartContext.TblDiscounts.Where(x => productCatIDs.Contains(x.FkProductId)).ToListAsync();

                foreach (var item in productDTOList)
                {
                    item.DeliveryCharges = deliveryChargesList.FirstOrDefault(x => x.FkProductcategoryId == item.ProductcategoryId).DeliveryCharges;

                    item.NormalDayDiscount = discounts.FirstOrDefault(x => x.FkProductId == item.ProductId).NormalDayDiscount;
                    item.AddWeekendsDiscount = discounts.FirstOrDefault(x => x.FkProductId == item.ProductId).AddWeekendsDiscount;
                }

                return productDTOList;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task AddProductToCart(ShoppingCartDTO shoppingCartDTO)
        {

            try
            {
                if (shoppingCartDTO.ProductId == 0)
                {
                    throw new Exception("Product id is required");
                }

                if (shoppingCartDTO.ProductQuantity == 0)
                {
                    throw new Exception("Product quantity cannot be zero");
                }

                var existingcart = await ShoppingCartContext.TblCarts.FirstOrDefaultAsync(x => x.FkProductId == shoppingCartDTO.ProductId);

                if (existingcart != null)
                {
                    shoppingCartDTO.ProductQuantity += existingcart.ProductQuantity;
                    shoppingCartDTO.CartID = existingcart.CartId;
                    await UpdateProductToCart(shoppingCartDTO);
                }
                else
                {
                    if (dayToday.DayOfWeek != DayOfWeek.Saturday && dayToday.DayOfWeek != DayOfWeek.Sunday)
                    {
                        shoppingCartDTO.AddWeekendsDiscount = 0;
                    }


                    var totalCost = shoppingCartDTO.CostPerUnit - ((shoppingCartDTO.CostPerUnit * (shoppingCartDTO.NormalDayDiscount.Value + shoppingCartDTO.AddWeekendsDiscount.Value)) / 100);

                    TblCart cart = new TblCart
                    {
                        FkProductId = shoppingCartDTO.ProductId,
                        ProductQuantity = shoppingCartDTO.ProductQuantity,
                        CostPerUnit = shoppingCartDTO.CostPerUnit,
                        NormalDayDiscount = shoppingCartDTO.NormalDayDiscount,
                        AddWeekendsDiscount = shoppingCartDTO.AddWeekendsDiscount,
                        TotalDiscount = (shoppingCartDTO.NormalDayDiscount.Value + shoppingCartDTO.AddWeekendsDiscount.Value),
                        DeliveryCharges = shoppingCartDTO.DeliveryCharges,
                        TotalCost = (totalCost * shoppingCartDTO.ProductQuantity) + shoppingCartDTO.DeliveryCharges
                    };

                    ShoppingCartContext.TblCarts.Add(cart);
                    await ShoppingCartContext.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<CartDTO> GetCartList()
        {
            try
            {
                var shoppingCartDTOs = await ShoppingCartContext.TblCarts.Select(x => new ShoppingCartDTO
                {
                    CartID = x.CartId,
                    ProductId = x.FkProductId,
                    ProductQuantity = x.ProductQuantity,
                    CostPerUnit = x.CostPerUnit,
                    NormalDayDiscount = x.NormalDayDiscount,
                    AddWeekendsDiscount = x.AddWeekendsDiscount,
                    TotalDiscount = x.TotalDiscount,
                    DeliveryCharges = x.DeliveryCharges,
                    TotalCost = x.TotalCost,
                    ProductName = x.FkProduct.ProductName
                }).ToListAsync();

                CartDTO cartDTO = new CartDTO();
                cartDTO.CartTotal = shoppingCartDTOs.Sum(s => s.TotalCost);
                if (cartDTO.CartTotal >= 25000)
                {
                    cartDTO.CartTotal = cartDTO.CartTotal - ((cartDTO.CartTotal * (10)) / 100);
                    cartDTO.FinalDiscount = true;

                }
                cartDTO.ShoppingCartDTO = shoppingCartDTOs;

                return cartDTO;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task UpdateProductToCart(ShoppingCartDTO shoppingCartDTO)
        {

            try
            {
                if (shoppingCartDTO.ProductId == 0)
                {
                    throw new Exception("Product id is required");
                }

                var existingcart = await ShoppingCartContext.TblCarts.FirstOrDefaultAsync(x => x.CartId == shoppingCartDTO.CartID);

                if (existingcart == null)
                {
                    throw new Exception("Product in cart does not exixt");
                }

                if (shoppingCartDTO.ProductQuantity == 0)
                {
                    await DeleteProductFromCart(existingcart.CartId);
                }
                else
                {

                    if (dayToday.DayOfWeek != DayOfWeek.Saturday && dayToday.DayOfWeek != DayOfWeek.Sunday)
                    {
                        shoppingCartDTO.AddWeekendsDiscount = 0;
                    }

                    var totalCost = shoppingCartDTO.CostPerUnit - ((shoppingCartDTO.CostPerUnit * (shoppingCartDTO.NormalDayDiscount.Value + shoppingCartDTO.AddWeekendsDiscount.Value)) / 100);

                    existingcart.FkProductId = shoppingCartDTO.ProductId;
                    existingcart.ProductQuantity = shoppingCartDTO.ProductQuantity;
                    existingcart.CostPerUnit = shoppingCartDTO.CostPerUnit;
                    existingcart.NormalDayDiscount = shoppingCartDTO.NormalDayDiscount;
                    existingcart.AddWeekendsDiscount = shoppingCartDTO.AddWeekendsDiscount;
                    existingcart.TotalDiscount = (shoppingCartDTO.NormalDayDiscount.Value + shoppingCartDTO.AddWeekendsDiscount.Value);
                    existingcart.DeliveryCharges = shoppingCartDTO.DeliveryCharges;
                    existingcart.TotalCost = (totalCost * shoppingCartDTO.ProductQuantity) + shoppingCartDTO.DeliveryCharges;

                    ShoppingCartContext.Entry(existingcart).State = EntityState.Modified;
                    await ShoppingCartContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task DeleteProductFromCart(int cartId)
        {
            try
            {
                var existingcart = await ShoppingCartContext.TblCarts.FirstOrDefaultAsync(x => x.CartId == cartId);

                if (existingcart == null)
                {
                    throw new Exception("Product in cart does not exixt");
                }

                ShoppingCartContext.TblCarts.Remove(existingcart);
                ShoppingCartContext.Entry(existingcart).State = EntityState.Deleted;
                await ShoppingCartContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        #region Bill Print Method definition
        public FileStreamResult BillGenerate(List<int> cartids)
        {
            try
            {
                var cartList = ShoppingCartContext.TblCarts.Where(x => cartids.Contains(x.CartId)).Select(x => new ShoppingCartDTO
                {
                    ProductCategoryName = x.FkProduct.FkProductcategory.ProductCategoryName,
                    ProductName = x.FkProduct.ProductName,
                    ProductQuantity = x.ProductQuantity,
                    CostPerUnit = x.CostPerUnit,
                    TotalDiscount = x.AddWeekendsDiscount.Value + x.NormalDayDiscount.Value,
                    DeliveryCharges = x.DeliveryCharges,
                    TotalCost = x.TotalCost
                }).ToList();

                Section section = CreateSection();
                DefineStyles();
                CreateHeader(section);

                CreateInvoiceTable(section, cartList);
                CreateFinalTotalTable(section, cartList.Sum(x => x.TotalCost));

                document.UseCmykColor = true;
                PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(true);
                pdfRenderer.Document = document;

                pdfRenderer.RenderDocument();

                // Save the document...
                MemoryStream stream = new MemoryStream();
                pdfRenderer.PdfDocument.Save(stream, false);

                FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/pdf");

                fileStreamResult.FileDownloadName = "Sample.pdf";

                return fileStreamResult;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        private void DefineStyles()
        {
            try
            {
                // Get the predefined style Normal.
                Style style = document.Styles["Normal"];
                style.Font.Name = "Verdana";
                style.Font.Size = 10;
                style = document.Styles[StyleNames.Header];
                style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

                // Create a new style called Table based on style Normal
                style = document.Styles.AddStyle("Table", "Normal");
                style.Font.Name = "Arial Narrow";
                style.Font.Size = 9;
                // Create a new style called Reference based on style Normal
                style = document.Styles.AddStyle("Reference", "Normal");

                // Create a new style called Reference based on style Normal
                style = document.Styles.AddStyle("InvoiceTitle", "Normal");
                style.Font.Name = "Arial Narrow";
                style.Font.Bold = true;
                style.Font.Size = 11;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        Section CreateSection()
        {
            Section section = document.AddSection();

            section.PageSetup.LeftMargin = "5mm";
            section.PageSetup.RightMargin = "5mm";
            section.PageSetup.TopMargin = "5mm";
            section.PageSetup.BottomMargin = "5mm";

            section.PageSetup.DifferentFirstPageHeaderFooter = true;

            return section;
        }

        private void CreateHeader(Section section)
        {
            try
            {
                TextFrame text = section.AddTextFrame();
                text.Width = "20.0cm";
                text.Height = "0.10cm";
                text.Left = ShapePosition.Left;
                text.Top = "0.10cm";

                Paragraph paragraph = text.AddParagraph();
                paragraph.Format.Alignment = ParagraphAlignment.Center;
                paragraph.AddFormattedText("Shopping Cart", TextFormat.Bold);

                paragraph.Format.Font.Size = 15;
                paragraph.Format.Borders.Width = 0;
                paragraph.Format.Borders.DistanceFromLeft = 5;
                paragraph.Format.Borders.DistanceFromTop = 3;
                paragraph.Format.Borders.DistanceFromRight = 5;
                paragraph.Format.Borders.DistanceFromBottom = 5;

                Paragraph paragraphDate = text.AddParagraph();
                paragraphDate.Format.Alignment = ParagraphAlignment.Left;
                paragraphDate.AddFormattedText("Date :-" + DateTime.Now.ToString(), TextFormat.Bold);
                paragraphDate.Format.Font.Size = 8;
                paragraphDate.Format.Borders.Width = 0;
                paragraphDate.Format.Borders.DistanceFromLeft = 5;
                paragraphDate.Format.Borders.DistanceFromTop = 5;
                paragraphDate.Format.Borders.DistanceFromRight = 5;
                paragraphDate.Format.Borders.DistanceFromBottom = 3;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        private void CreateInvoiceTable(Section section, List<ShoppingCartDTO> cartList)
        {
            //table
            Paragraph paraSpace = section.AddParagraph();
            paraSpace.AddLineBreak();
            paraSpace.AddFormattedText("", TextFormat.Bold);
            paraSpace.Format.Font.Size = 12;

            paraSpace.AddLineBreak();
            paraSpace.AddLineBreak();

            Table invoiceTable = new Table();
            invoiceTable = section.AddTable();

            invoiceTable.Style = "Table";
            invoiceTable.Borders.Width = 1;
            invoiceTable.Borders.Left.Width = 1;
            invoiceTable.Borders.Right.Width = 1;
            invoiceTable.Rows.LeftIndent = 1;
            invoiceTable.Borders.Top.Width = 1;


            // Before you can add a row, you must define the columns
            invoiceTable.AddColumn("2cm");
            invoiceTable.AddColumn("3cm");//2
            invoiceTable.AddColumn("3cm");//2
            invoiceTable.AddColumn("3cm");//3
            invoiceTable.AddColumn("2cm");//0.9

            invoiceTable.AddColumn("2cm");//3
            invoiceTable.AddColumn("3cm");//3
            invoiceTable.AddColumn("2cm");//2


            foreach (Column c in invoiceTable.Columns)
            {
                c.Format.Alignment = ParagraphAlignment.Right;
            }

            // Create the header of the table

            Row row = invoiceTable.AddRow();
            row.HeadingFormat = true;


            //row.Format.Alignment = ParagraphAlignment.Center;

            row.Format.Font.Bold = true;
            row.Format.Font.Size = 11;
            row.Cells[0].AddParagraph("Sr.No");
            row.Cells[1].AddParagraph("Product");
            row.Cells[2].AddParagraph("Category");
            row.Cells[3].AddParagraph("Quantity");
            row.Cells[4].AddParagraph("Cost per unit");

            row.Cells[5].AddParagraph("Discount if any");
            row.Cells[6].AddParagraph("Delivery charges");
            row.Cells[7].AddParagraph("Total Price per product");

            row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[2].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[3].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[4].Format.Alignment = ParagraphAlignment.Left;

            row.Cells[5].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[6].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[7].Format.Alignment = ParagraphAlignment.Left;

            row.Shading.Color = Color.FromRgb(41, 128, 185);
            row.Format.Font.Color = Color.FromRgb(255, 255, 255);

            int count = 1;
            foreach (var item in cartList)
            {
                Row cartRow = invoiceTable.AddRow();

                cartRow.Format.Font.Size = 10;
                cartRow.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                cartRow.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                cartRow.Cells[2].Format.Alignment = ParagraphAlignment.Left;
                cartRow.Cells[3].Format.Alignment = ParagraphAlignment.Left;
                cartRow.Cells[4].Format.Alignment = ParagraphAlignment.Left;

                cartRow.Cells[5].Format.Alignment = ParagraphAlignment.Left;
                cartRow.Cells[6].Format.Alignment = ParagraphAlignment.Left;
                cartRow.Cells[7].Format.Alignment = ParagraphAlignment.Left;


                cartRow.Cells[0].AddParagraph(count.ToString());
                cartRow.Cells[1].AddParagraph(item.ProductName);
                cartRow.Cells[2].AddParagraph(item.ProductCategoryName);
                cartRow.Cells[3].AddParagraph(item.ProductQuantity.ToString());
                cartRow.Cells[4].AddParagraph(item.CostPerUnit.ToString());

                cartRow.Cells[5].AddParagraph(item.TotalDiscount.ToString());
                cartRow.Cells[6].AddParagraph(item.DeliveryCharges.ToString());
                cartRow.Cells[7].AddParagraph(item.TotalCost.ToString());

                count++;
            }
        }

        private void CreateFinalTotalTable(Section section, decimal totalCost)
        {
            Paragraph finalTotalSpace = section.AddParagraph();
            finalTotalSpace.AddLineBreak();
            finalTotalSpace.AddFormattedText("", TextFormat.Bold);
            finalTotalSpace.Format.Font.Size = 12;

            //  finalTotalSpace.AddLineBreak();

            Table totalTable = new Table();
            totalTable = section.AddTable();

            totalTable.Style = "Table";
            totalTable.Borders.Width = 1;
            totalTable.Borders.Left.Width = 1;
            totalTable.Borders.Right.Width = 1;
            totalTable.Rows.LeftIndent = 1;
            totalTable.Borders.Top.Width = 1;


            // Before you can add a row, you must define the columns
            totalTable.AddColumn("20cm");

            foreach (Column c in totalTable.Columns)
            {
                c.Format.Alignment = ParagraphAlignment.Right;
            }

            // Create the header of the table

            Row totalfRow = totalTable.AddRow();
            totalfRow.HeadingFormat = true;


            //row.Format.Alignment = ParagraphAlignment.Center;

            totalfRow.Format.Font.Bold = true;
            totalfRow.Format.Font.Size = 11;

            if (totalCost >= 25000)
            {
                totalCost = totalCost - ((totalCost * (10)) / 100);
                totalfRow.Cells[0].AddParagraph("Total bill amount after 10% discount");
            }
            else
            {
                totalfRow.Cells[0].AddParagraph("Total bill amount");
            }


            totalfRow.Cells[0].Format.Alignment = ParagraphAlignment.Left;

            totalfRow.Shading.Color = Color.FromRgb(41, 128, 185);
            totalfRow.Format.Font.Color = Color.FromRgb(255, 255, 255);

            Row row = totalTable.AddRow();
            row.Format.Font.Size = 10;
            row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[0].AddParagraph(totalCost.ToString() + " Rs");
        }
     
        #endregion

    }
}
