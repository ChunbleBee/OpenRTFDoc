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
        Assert.DoesNotThrow(() => { docGroup = Parser.Parse(rtfStr); });
        Assert.DoesNotThrow(() => { documentNode = DomBuilder.Build(docGroup); })
    }
}