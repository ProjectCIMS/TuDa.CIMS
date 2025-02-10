// See https://aka.ms/new-console-template for more information

using QuestPDF.Companion;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using TuDa.CIMS.Api.Documents;
using TuDa.CIMS.Api.Test.Faker;
using TuDa.CIMS.Shared.Models;

QuestPDF.Settings.License = LicenseType.Community;

var invoice = new InvoiceFaker();

var tables = new InvoiceTablesDocument(invoice);

// var cover = new InvoiceCoverDocument(
//     invoice,
//     Image.FromFile("./logo.png"),
//     new AdditionalInvoiceInformation
//     {
//         InvoiceNumber = "Invoice1234",
//         DueDate = DateOnly.FromDateTime(DateTime.Today),
//     }
// );

//var document = Document.Merge(cover, tables).UseContinuousPageNumbers();
var document = tables;

await document.ShowInCompanionAsync();
//document.GeneratePdfAndShow();
