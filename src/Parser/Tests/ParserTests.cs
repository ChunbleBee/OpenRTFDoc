namespace ParserTests;

using System.Text;
using NUnit.Framework;
using NUnit;
using Parser;
using RtfModels;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestBasicParsing()
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

        Tuple<string, Type, string?>[] expectedDocChildren = {
            new("rtf", typeof(DestinationWord), "1"),
            new("ansi", typeof(FlagWord), null),
            new("deff", typeof(ValueWord), "0"),
            new(string.Empty, typeof(Group), null),
            new(string.Empty, typeof(Group), null),
            new(string.Empty, typeof(Group), null),
            new(string.Empty, typeof(Group), null),
            new("widowctrl", typeof(FlagWord), null),
            new("ftnbj", typeof(FlagWord), null),
            new("sectd", typeof(FlagWord), null),
            new("linex", typeof(ValueWord), "0"),
            new("endnhere", typeof(FlagWord), null),
            new("pard", typeof(FlagWord), null),
            new("plain", typeof(FlagWord), null),
            new("fs", typeof(ValueWord), "20"),
            new(string.Empty, typeof(Run), "This is plain text."),
            new("par", typeof(SymbolWord), null)
        };

        Tuple<string, Type, string?>[,] expectedFtblChildren = {
            {
                new("f", typeof(ValueWord), "0"),
                new("froman", typeof(FlagWord), null),
                new(string.Empty, typeof(Run), "Tms Rmn;")
            },
            {
                new("f", typeof(ValueWord), "1"),
                new("fdecor", typeof(FlagWord), null),
                new(string.Empty, typeof(Run), "Symbol;")
            },
            {
                new("f", typeof(ValueWord), "2"),
                new("fswiss", typeof(FlagWord), null),
                new(string.Empty, typeof(Run), "Helv;")
            }
        };

        Tuple<string, Type, string?>[] expectedClrTbl = {
            new("colortbl", typeof(DestinationWord), null),
            new(string.Empty, typeof(Run), ";"),
            new("red", typeof(ValueWord), "0"),
            new("green", typeof(ValueWord), "0"),
            new("blue", typeof(ValueWord), "0"),
            new(string.Empty, typeof(Run), ";"),
            new("red", typeof(ValueWord), "0"),
            new("green", typeof(ValueWord), "0"),
            new("blue", typeof(ValueWord), "255"),
            new(string.Empty, typeof(Run), ";"),
            new("red", typeof(ValueWord), "0"),
            new("green", typeof(ValueWord), "255"),
            new("blue", typeof(ValueWord), "255"),
            new(string.Empty, typeof(Run), ";"),
            new("red", typeof(ValueWord), "0"),
            new("green", typeof(ValueWord), "255"),
            new("blue", typeof(ValueWord), "0"),
            new(string.Empty, typeof(Run), ";"),
            new("red", typeof(ValueWord), "255"),
            new("green", typeof(ValueWord), "0"),
            new("blue", typeof(ValueWord), "255"),
            new(string.Empty, typeof(Run), ";"),
            new("red", typeof(ValueWord), "255"),
            new("green", typeof(ValueWord), "0"),
            new("blue", typeof(ValueWord), "0"),
            new(string.Empty, typeof(Run), ";"),
            new("red", typeof(ValueWord), "255"),
            new("green", typeof(ValueWord), "255"),
            new("blue", typeof(ValueWord), "0"),
            new(string.Empty, typeof(Run), ";"),
            new("red", typeof(ValueWord), "255"),
            new("green", typeof(ValueWord), "255"),
            new("blue", typeof(ValueWord), "255"),
            new(string.Empty, typeof(Run), ";"),
        };

        Group doc = new();
        Assert.DoesNotThrow(
            () => { doc = Parser.Parse(rtfstr); }
        );

        TestContext.WriteLine($"Returned RTF String: {doc}");
        TestContext.Write($"Children: [");
        foreach (var child in doc!.Children)
        {
            TestContext.Write($"{child}-{child.Name}, ");
        }
        TestContext.WriteLine("]");

        Assert.That(doc.Children, Has.Count.EqualTo(17));

        // Test direct children correctly parsed
        for(int i = 0; i < doc.Children.Count; i++)
        {
            TestContext.WriteLine($"Testing Outer Doc #{i + 1}: {doc.Children[i].GetType()}-{doc.Children[i].Name}");
            Assert.Multiple(() =>
            {
                Assert.That(doc.Children[i].Name, Is.EqualTo(expectedDocChildren[i].Item1));
                Assert.That(doc.Children[i].GetType(), Is.EqualTo(expectedDocChildren[i].Item2));
                Assert.That(doc.Children[i].Param, Is.EqualTo(expectedDocChildren[i].Item3));
            });
        }

        // Test embedded group parsing
        Group fonttbl = (Group)doc.Children[3];
        Assert.That(fonttbl.Children, Has.Count.EqualTo(4));
        Assert.Multiple(() =>
        {
            Assert.That(fonttbl.Children[0].Name, Is.EqualTo("fonttbl"));
            Assert.That(fonttbl.Children[0].GetType(), Is.EqualTo(typeof(DestinationWord)));
            Assert.That(fonttbl.Children[0].Param, Is.EqualTo(null));
        });

        for (int i = 1; i < fonttbl.Children.Count; i++)
        {
            TestContext.WriteLine($"Testing Font Table Group #{i}");
            Assert.That(fonttbl.Children[i].GetType(), Is.EqualTo(typeof(Group)));
            Group fgroup = (Group)fonttbl.Children[i];

            Assert.That(fgroup.Children, Has.Count.EqualTo(3));
            for(int j = 0; j < fgroup.Children.Count; j++)
            {
                var actualChild = fgroup.Children[j];
                var expectedChild = expectedFtblChildren[i - 1, j];

                Assert.Multiple(() =>
                {
                    Assert.That(actualChild.Name, Is.EqualTo(expectedChild.Item1));
                    Assert.That(actualChild.GetType(), Is.EqualTo(expectedChild.Item2));
                    Assert.That(actualChild.Param, Is.EqualTo(expectedChild.Item3));
                });
            }
        }

        Group colortbl = (Group)doc.Children[4];
        for (int i = 0; i < colortbl.Children.Count; i++)
        {
            var actualChild = colortbl.Children[i];
            var expectedChild = expectedClrTbl[i];
    
            TestContext.WriteLine($"Testing Color Table #{i + 1}: {actualChild.GetType()}-{actualChild.Name}: {actualChild.Param}");
            Assert.Multiple(() =>
            {
                Assert.That(actualChild.Name, Is.EqualTo(expectedChild.Item1));
                Assert.That(actualChild.GetType(), Is.EqualTo(expectedChild.Item2));
                Assert.That(actualChild.Param, Is.EqualTo(expectedChild.Item3));
            });
        }
    }
}