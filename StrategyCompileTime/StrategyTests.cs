using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace StrategyCompileTime
{
    public enum OutputFormat
    {
        Markdown,
        Html
    }

    // <ul><li></li><li></li></ul>
    public interface IListStrategy
    {
        void Start(StringBuilder sb);
        void End(StringBuilder sb);
        void AddListItem(StringBuilder sb, string item);
    }

    public class HtmlListStrategy : IListStrategy
    {
        public void Start(StringBuilder sb)
        {
            sb.AppendLine("<ul>");
        }

        public void End(StringBuilder sb)
        {
            sb.AppendLine("</ul>");
        }

        public void AddListItem(StringBuilder sb, string item)
        {
            sb.AppendLine($" <li>{item}</li>");
        }
    }

    public class MarkdownListStrategy : IListStrategy
    {
        public void AddListItem(StringBuilder sb, string item)
        {
            sb.AppendLine($" * {item}");
        }

        public void End(StringBuilder sb)
        {
        }

        public void Start(StringBuilder sb)
        {
        }
    }

    public class TextProcessor<LS> where LS : IListStrategy, new()
    {
        private StringBuilder _stringBuilder = new StringBuilder();
        private IListStrategy _listStrategy = new LS();

        public void SetOutputFormat(OutputFormat format)
        {
            switch (format)
            {
                case OutputFormat.Markdown:
                    _listStrategy = new MarkdownListStrategy();
                    break;
                case OutputFormat.Html:
                    _listStrategy = new HtmlListStrategy();
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Format {format} not supported!");
            }
        }

        public void AppendList(IEnumerable<string> items)
        {
            _listStrategy.Start(_stringBuilder);
            foreach (var item in items)
            {
                _listStrategy.AddListItem(_stringBuilder, item);
            }
            _listStrategy.End(_stringBuilder);
        }

        public void Clear()
        {
            _stringBuilder.Clear();
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }
    }

    public class StrategyTests
    {
        [Test]
        public void Output_markdown()
        {
            var tp = new TextProcessor<MarkdownListStrategy>();
            tp.AppendList(new[] { "foo", "bar", "baz" });

            Debug.WriteLine(tp);
            var result = $"{tp}";
            Assert.False(string.IsNullOrEmpty(result));
        }

        [Test]
        public void Output_html()
        {
            var tp = new TextProcessor<HtmlListStrategy>();
            tp.AppendList(new[] { "foo", "bar", "baz" });

            Debug.WriteLine(tp);
            var result = $"{tp}";
            Assert.False(string.IsNullOrEmpty(result));
        }
    }
}