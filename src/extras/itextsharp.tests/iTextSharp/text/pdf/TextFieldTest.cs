/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2022 iText Group NV
    Authors: iText Software.

    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License version 3
    as published by the Free Software Foundation with the addition of the
    following permission added to Section 15 as permitted in Section 7(a):
    FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
    ITEXT GROUP. ITEXT GROUP DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
    OF THIRD PARTY RIGHTS
    
    This program is distributed in the hope that it will be useful, but
    WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
    or FITNESS FOR A PARTICULAR PURPOSE.
    See the GNU Affero General Public License for more details.
    You should have received a copy of the GNU Affero General Public License
    along with this program; if not, see http://www.gnu.org/licenses or write to
    the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
    Boston, MA, 02110-1301 USA, or download the license from the following URL:
    http://itextpdf.com/terms-of-use/
    
    The interactive user interfaces in modified source and object code versions
    of this program must display Appropriate Legal Notices, as required under
    Section 5 of the GNU Affero General Public License.
    
    In accordance with Section 7(b) of the GNU Affero General Public License,
    a covered work must retain the producer line in every PDF that is created
    or manipulated using iText.
    
    You can be released from the requirements of the license by purchasing
    a commercial license. Buying such a license is mandatory as soon as you
    develop commercial activities involving the iText software without
    disclosing the source code of your own applications.
    These activities include: offering paid services to customers as an ASP,
    serving PDFs on the fly in a web application, shipping iText with a closed
    source product.
    
    For more information, please contact iText Software Corp. at this
    address: sales@itextpdf.com
 */
using System;
using System.IO;
using iTextSharp.testutils;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.events;
using NUnit.Framework;

namespace itextsharp.tests.iTextSharp.text.pdf {
    public class TextFieldTest {
        private const string CMP_FOLDER = @"..\..\..\resources\text\pdf\TextFieldTest\";
        private const string OUTPUT_FOLDER = @"TextFieldTest\";

        [TestFixtureSetUp]
        public static void Init() {
            Directory.CreateDirectory(OUTPUT_FOLDER);
        }

        [Test]
        public virtual void TestVisibleTopChoice() {
            int[] testValues = new int[] {-3, 0, 2, 3};
            int[] expectedValues = new int[] {-1, 0, 2, -1};

            for (int i = 0; i < testValues.Length; i++) {
                VisibleTopChoiceHelper(testValues[i], expectedValues[i], "textfield-top-visible-" + i + ".pdf");
            }
        }


        private void VisibleTopChoiceHelper(int topVisible, int expected, String file) {
            Document document = new Document();
            FileStream fos = new FileStream(OUTPUT_FOLDER + file, FileMode.Create);
            PdfWriter writer = PdfWriter.GetInstance(document, fos);
            document.Open();

            TextField textField = new TextField(writer, new Rectangle(380, 560, 500, 610), "testListBox");

            textField.Visibility = BaseField.VISIBLE;
            textField.Rotation = 0;

            textField.FontSize = 14f;
            textField.TextColor = BaseColor.MAGENTA;

            textField.BorderColor = BaseColor.BLACK;
            textField.BorderStyle = PdfBorderDictionary.STYLE_SOLID;

            textField.Font = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.WINANSI, BaseFont.EMBEDDED);
            textField.BorderWidth = BaseField.BORDER_WIDTH_THIN;

            writer.RunDirection = PdfWriter.RUN_DIRECTION_LTR;

            textField.Choices = new String[] {"one", "two", "three"};
            textField.ChoiceExports = new String[] {"1", "2", "3"};

            //choose the second item
            textField.ChoiceSelection = 1;
            //set the first item as the visible one
            textField.VisibleTopChoice = topVisible;

            Assert.AreEqual(expected, textField.VisibleTopChoice);

            PdfFormField field = textField.GetListField();

            writer.AddAnnotation(field);

            document.Close();

            // compare
            CompareTool compareTool = new CompareTool();
            String errorMessage = compareTool.CompareByContent(OUTPUT_FOLDER + file, CMP_FOLDER + file, OUTPUT_FOLDER, "diff");
            if (errorMessage != null) {
                Assert.Fail(errorMessage);
            }
        }
    }
}
