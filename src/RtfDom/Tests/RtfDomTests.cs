namespace RtfDomTests;

using NUnit.Framework;
using RtfDom;
using RtfParser;
using RtfModels;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestDomInterpreter()
    {
        string rtfstr = @"
        {
            \rtf1
            \ansi
            \deff0
            {\fonttbl{\f0\froman Tms Rmn;}{\f1\fdecor Symbol;}{\f2\fswiss Helv;}}
            {\colortbl;
                \red0\green0\blue0;
                \red0\green0\blue255;
                \red0\green255\blue255;
                \red0\green255\blue0;
                \red255\green0\blue255;
                \red255\green0\blue0;
                \red255\green255\blue0;
                \red255\green255\blue255;
            }
            {\stylesheet{\fs20 \snext0 Normal;}}
            {\info
                {\author John Doe}
                {\creatim\yr1990\mo7\dy30\hr10\min48}
                {\version1}
                {\edmins0}
                {\nofpages1}
                {\nofwords0}
                {\nofchars0}
                {\vern8351}
            }
            \widowctrl\ftnbj \sectd\linex0\endnhere \pard\plain \fs20 This is plain text.\par
        }";

        Group? docGroup = null;
        DocumentNode? docNode = null;
        Assert.DoesNotThrow(() => { docGroup = Parser.Parse(rtfstr); });
        Assert.DoesNotThrow(() => { docNode = DomBuilder.Build(docGroup!); });
        TestContext.WriteLine($"Document: {docNode}");
    }

    [Test]
    public void TestDomInterpreterOldListStyle()
    {
        string rtfstr = @"
        {
            \rtf1
            \pard
            This is a paragraph of text.\par
            # Should provide a formatting 
            {\pntext\f0 1.\tab}{\*\pn\pnlvlblt\pnf0\pnindent0{\pntxtb .}}This is a paragraph with a numbered list item.\par
                {\pntext\f0 1.\tab}{\pntext\f0 1.\tab}{\*\pn\pnlvlbody\pnf0\pnindent0\pnstart1{\pntxta .}}This is a second level list item.\par
                {\pntext\f0 1.\tab}This is another second level list item.\par
                    {\pntext\f0 1.\tab}{\*\pn\pnlvlblt\pnf0\pnindent0{\pntxtb .}}\b This is another \i paragraph with a numbered \b0 list item.\par
        }";

        Group? docGroup = null;
        DocumentNode? docNode = null;
        Assert.DoesNotThrow(() => { docGroup = Parser.Parse(rtfstr); });
        Assert.DoesNotThrow(() => { docNode = DomBuilder.Build(docGroup!); });
        TestContext.WriteLine($"Document: {docNode}");
    }
}