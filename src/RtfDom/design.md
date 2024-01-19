# RTF DOM Design
__
## DOM Objects
OpenRTFDoc will follow a similar convention to many other C# DOM libraries.
That is:
* A Node is the base class.
    * Each element is represented by a node.
    * A Node will contain a list of child Nodes, called Children.
        * The order of the Nodes in Children are representational.
            * E.g., if Node A's Children = [Node C, Node D, Node B], then the flattened representation will be [A, C, D, B]
    * A Node may contain a link to it's Parent.
    * A Node contains a list of Attributes.
    * A Node contains a string, InnerText, that represents the plain text of that representation.
* An Attribute is the base wrapper for element modifiers.
* The DOM is a tree hierarchy of Nodes.

## Converting RtfModel.Group into a DOM
Because of some nuances in RTF parsing, this DOM will be flatter than many others (say HTML).
* Non-Destination Groups create child Nodes.
* Destination Groups create Formatting Attributes.
    * Formatting Attributes are saved and applied to the appropriate target node (generally, the next text node).
* Toggles create a new child text node.
    * All other settings are maintained from the parent node.
* Values create Attributes for the current node.


## Converting from DOM into RTF Strings
This process will not return a one to one plain text result, but should return the same _visual result_ in most RTF Readers.