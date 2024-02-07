using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace OpenXml.Tools
{
    public static class ScoreWriter
    {
        private static int measuresPerPage = 10; // 10 measures per page for quantet
        private static WordprocessingDocument docx;
        private static int fontType = 1;
        private static int partNum; // 部別數量

        public static void Write(string savePath, List<List<List<char>>> score)
        {
            partNum = score.First().Count;
            measuresPerPage = GetMeasuresPerPage(partNum);
            using (docx = WordprocessingDocument.Create(savePath, WordprocessingDocumentType.Document))
            {
                // Add a main document part. 
                docx.AddMainDocumentPart();
                docx.MainDocumentPart.Document = new Document(new Body());
                ScoreToTables(score);
            }
        }

        private static int GetMeasuresPerPage(int partNum)
        {
            if (partNum == 1)
                return 23 * 2; // 23 tables per page, and 2 measures for each table
            else if (partNum == 2)
                return 11 * 2;
            else if (partNum == 3)
                return 7 * 2;
            else if (partNum == 4)
                return 6 * 2;
            else if (partNum == 5)
                return 5 * 2;
            else if (partNum == 6)
                return 4 * 2;
            else if (partNum == 7 || partNum == 8)
                return 3 * 2;
            else if (partNum >= 9 && partNum <= 13)
                return 2 * 2;
            return 1;
        }

        private static void ScoreToTables(List<List<List<char>>> score)
        {
            for (int measure = 0; measure < score.Count; measure++)
            {
                if (measure != 0 && measure % measuresPerPage == 0) // 每n個小節換頁
                    docx.MainDocumentPart.Document.Body.Append(new Paragraph(new Run(new Break() { Type = BreakValues.Page })));

                if (measure % 2 == 0) // 一排兩小節
                {
                    docx.MainDocumentPart.Document.Body.Append(CreateTable());
                    ParagraphProperties pPr = new ParagraphProperties(
                        new SpacingBetweenLines()
                        {
                            Line = "240", // 固定行高12pt
                            LineRule = LineSpacingRuleValues.Exact
                        });
                    docx.MainDocumentPart.Document.Body.Append(new Paragraph(pPr));
                }

                Table _table = docx.MainDocumentPart.Document.Body.Elements<Table>().Last();
                TableRow _row = _table.Elements<TableRow>().ElementAt(0);
                TableCell _cell = _row.Elements<TableCell>().ElementAt(measure % 2);

                for (int part = 0; part < partNum; part++)
                {
                    if (partNum == 1) // 單聲部
                        AddEmptyParagraph(ref _cell, 12, LineSpacingRuleValues.Auto);
                    else if (part == 0)
                    {
                        AddEmptyParagraph(ref _cell, 5, LineSpacingRuleValues.Exact);
                        AddEmptyParagraph(ref _cell, 12, LineSpacingRuleValues.Auto);
                    }
                    else
                    {
                        AddEmptyParagraph(ref _cell, 8, LineSpacingRuleValues.Exact);
                        AddEmptyParagraph(ref _cell, 12, LineSpacingRuleValues.Auto);
                    }

                    Paragraph _p = _cell.Elements<Paragraph>().Last();

                    if (part < score[measure].Count)
                    {
                        for (int element = 0; element < score[measure][part].Count; element++)
                        {
                            char note = score[measure][part][element];
                            _p.Append(GetRun(note));
                        }
                    }
                }
            }
        }

        private static void AddEmptyParagraph(ref TableCell cell, int points, LineSpacingRuleValues rule)
        {
            string line = (20 * points).ToString(); // interpreted as twentieths of a point

            ParagraphProperties pPr = new ParagraphProperties(
                new SpacingBetweenLines()
                {
                    Line = line,
                    LineRule = rule
                });
            Paragraph p = new Paragraph(pPr);
            cell.Append(p);
        }

        private static Run GetRun(char note)
        {
            if (note == '+')
            {
                fontType = 2;
                return new Run(new Text(""));
            }
            Run r = new Run(new Text(note.ToString()));
            SetRunFont(r, fontType);
            fontType = 1;

            return r;
        }

        private static Table CreateTable()
        {
            Table table = new Table();
            // Append the TableProperties object to the empty table.
            table.AppendChild<TableProperties>(CreateTableProperties());

            // Create a row.
            TableRow tr = new TableRow();
            tr.AppendChild<TableRowProperties>(CreateTableRowProperties());

            // Create a cell.
            TableCell tc1 = new TableCell();
            TableCell tc2 = new TableCell();

            // Append the table cell to the table row.
            tr.Append(tc1);
            tr.Append(tc2);

            // Append the table row to the table.
            table.Append(tr);

            return table;
        }

        private static TableRowProperties CreateTableRowProperties()
        {
            return new TableRowProperties(
                    new TableRowHeight() {
                        HeightType = HeightRuleValues.Auto
                    });
        }

        private static TableProperties CreateTableProperties()
        {
            // Create a TableProperties object and specify its border information.
            return new TableProperties(
                new TableBorders(
                    new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.None) },
                    new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.None) },
                    new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.BasicThinLines), Size = 1 },
                    new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.BasicThinLines), Size = 1 },
                    new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.None) },
                    new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.BasicThinLines), Size = 1 }
                )
            );
        }

        private static void SetRunFont(Run run, int fontType)
        {
            // Set the font to Arial to the first Run.
            // Use an object initializer for RunProperties and rPr.
            RunProperties rPr = fontType == 1 ? new RunProperties(new RunFonts() { Ascii = "nckuhc_font_01" }) : new RunProperties(new RunFonts() { Ascii = "nckuhc_font_02" });
            run.PrependChild<RunProperties>(rPr);
        }
    }
}
