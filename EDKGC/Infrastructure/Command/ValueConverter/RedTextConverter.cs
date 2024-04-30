using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace EDKGC.Infrastructure.Command.ValueConverter
{
    public class RedTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text)
            {
                string[] parts = text.Split(new string[] { "!!!" }, StringSplitOptions.None);

                FlowDocument flowDocument = new FlowDocument();

                bool isRed = false;

                foreach (string part in parts)
                {
                    if (isRed)
                    {
                        flowDocument.Blocks.Add(new Paragraph(new Run(part) { Foreground = Brushes.Red }));
                    }
                    else
                    {
                        flowDocument.Blocks.Add(new Paragraph(new Run(part)));
                    }

                    isRed = !isRed;
                }

                return flowDocument;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}