/****************************************************
    Corbis UI JS Serializer
***************************************************/


/****************************************************
JS SERIALIZER 
                
Utility to serialize a javascript object into
XML string. 
        
Can serialize the following:
- string
- array
- date
- object
- function (serializes function as XML CDATA)
        
USED BY: CorbisUI.CookieEvents
        
Mainly built because you can't normally store
javascript in a cookie. But you can store XML
string. 
        
See Chris for more info.
            
****************************************************/

/** EXAMPLES

// TO SERIALIZE
var test = CorbisUI.JSSerializer.serialize({
runMe: function(){ 
this.cool(); 
},
cool: function(){ 
alert(this.myString); 
},
myString: 'wassup foo!',
myNumber: 2567,
myBoolean: true,
myObject: {
secondObject: "howdy"
},
myArray: ["bob","debbie"],
myDate: new Date()
});
    
// test obj serialized looks like this
        
"<undefined type="object">
<runMe type="function">
<![CDATA[function () { this.cool(); }]]>
</runMe>
<cool type="function">
<![CDATA[function () { alert(this.myString); }]]>
</cool>
<myString type="string">wassup foo!</myString>
<myNumber type="number">2567</myNumber>  
<myBoolean type="boolean">true</myBoolean>  
<myObject type="object">  
<secondObject type="string">howdy</secondObject>
</myObject> 
<myArray type="array">
<index0 type="string">bob</index0>
<index1 type="string">debbie</index1>  
</myArray>  
<myDate type="date">Wednesday, February 25, 2009 9:03:46 AM</myDate> 
</undefined> "
    
this can now be stored in cookie.
        
// TO DESERIALIZE
var deser = CorbisUI.JSSerializer.deserialize(test);
        
deser will now be a usable javascript object
*/

CorbisUI.JSSerializer = new Class({
    Implements: [Options],
    options: {
        use_encryption: false,
        use_compress: false
    },
    isIE: false,
    isMoz: false,

    initialize: function(options) {
        if (options) this.setOptions(options);
        this.isIE = Browser.Engine.trident;
        this.isMoz = Browser.Engine.gecko;
    },
    /**
    * Serialize a JS object into an XML String for storage / transmission
    * (i.e. cookie, download etc.)
    * 
    * objectToSerialize (object) : The object to be serialized
    *        objectName (object) : (Optional) Name of the object being passed in
    *       indentSpace (object) : (Optional) Use this as an indentSpace
    *
    * returns a xml string specifying the object
    */
    serialize: function(objectToSerialize, objectName, indentSpace) {
        //console.log('serialize');
        indentSpace = indentSpace ? indentSpace : '';

        var type = this.GetTypeName(objectToSerialize).toLowerCase();

        var s = indentSpace + '<' + objectName + ' type="' + type + '">';

        switch (type) {
            case "number":
            case "string":
            case "boolean":
                s += objectToSerialize;
                break;

            case "date":
                s += objectToSerialize.toLocaleString();
                break;

            case "function":
                s += "\n";
                s += "<![CDATA[" + objectToSerialize + "]]>";
                s += indentSpace;
                break;

            case "array":
                s += "\n";

                objectToSerialize.each(function(item, index) {
                    s += this.serialize(item, 'index' + index, indentSpace + "   ");
                }, this);

                s += indentSpace;
                break;

            default:
                s += "\n";

                for (var name in objectToSerialize) {
                    s += this.serialize(objectToSerialize[name], name, indentSpace + "   ");
                };

                s += indentSpace;
                break;

        }

        s += "</" + objectName + ">\n";

        return s;
    },

    /**
    * Deserialize a serialized XML object into a javascript object
    * Uses deserial recursively to rebuild the javascript 
    *
    * SEE: deserial
    *
    * XmlText (String) : xml string to be deserialized
    *
    * return Object
    */
    deserialize: function(XmlText) {
        //console.log('deserialize');
        var _doc = this.getDom(XmlText);
        return this.deserial(_doc.childNodes[0]);
    },

    /**
    * Get the DOM object from an XML doc
    * NB: Works for IE and Mozilla
    *
    * strXml (String) : xml string to be deserialized
    *
    * returns (Dom parser) appropriate to browser
    */
    getDom: function(strXml) {
        //console.log('getDom');
        if (!this.isIE) var parser = new DOMParser();
        var _doc = (this.isIE) ? new ActiveXObject("Msxml2.DOMDocument.3.0") : parser.parseFromString(strXml, "text/xml");

        if (this.isIE) _doc.loadXML(strXml);

        return _doc;
    },

    /**
    * Deserialize an XML DOM object into a javascript object
    * 
    * NOTE: This function uses recursion
    *
    * domObject (Object) : The DOM object to deserialize into a JS Object
    */
    deserial: function(domObject) {
        //console.log('deserial');
        var retObj;
        var nodeType = this.getNodeType(domObject);

        if (this.isSimpleVar(nodeType)) {
            var simpleReturn = (this.isIE) ? this.StringToObject(domObject.text, nodeType) : this.StringToObject(domObject.textContent, nodeType);
            return simpleReturn;
        }

        switch (nodeType) {
            case "array":
                return this.deserializeArray(domObject);
                break;

            case "function":
                return this.deserializeFunction(domObject);
                break;

            case "object":
            default:
                try {
                    retObj = eval("new " + nodeType + "()");
                } catch (e) {
                    // create generic class
                    retObj = new Object();
                }
                break;
        }


        for (var i = 0; i < domObject.childNodes.length; i++) {
            var Node = domObject.childNodes[i];
            retObj[Node.nodeName] = this.deserial(Node);
        }

        return retObj;
    },

    /**
    * Check if the current element is one of the primitive data types
    *
    * type (String) : The "type" attribute of the current node
    *
    * returns (boolean)
    */
    isSimpleVar: function(type) {
        //console.log('isSimpleVar');
        var criteria = ["number", "string", "boolean", "date"];
        if (criteria.contains(type)) return true;

        return false;
    },

    /**
    * Convert a string to an object
    *
    * text (String) : The text to parse into the new object
    * type (String) : The type of object that you wish to parse TO
    */
    StringToObject: function(text, type) {
        //console.log('StringToObject: ' + type);
        var retObj = null;

        switch (type) {

            case "number":
                var outNum = (text.contains('.')) ? text.toFloat() : text.toInt();
                return outNum;

            case "string":
                return text;

            case "date":
                return new Date(text);

            case "boolean":

                if (text == "true" || text == "True") {
                    return true;
                }
                else {
                    return false;
                }

                return parseBool(text);
        }

        return retObj;
    },

    /**
    * Get the name of an object by extracting it from it's constructor attribute
    *
    * obj (Object) : The object for which the name is to be found
    *
    * returned classname (string)
    */
    getClassName: function(obj) {
        //console.log('getClassName');
        try {
            var ClassName = obj.constructor.toString();
            ClassName = ClassName.substring(ClassName.indexOf("function") + 8, ClassName.indexOf('(')).replace(/ /g, '');
            return ClassName;
        }
        catch (e) {
            return "NULL";
        }
    },

    /**
    * Get the type of Object by checking against the Built-in objects.
    * If no built in object is found, call getClassName
    *
    * SEE: getClassName
    *
    * obj (Object) - The object for which the type is to be found
    * 
    * returns type (String)
    */
    GetTypeName: function(obj) {
        //console.log('GetTypeName');
        var type = $type(obj);
        if (this.isSimpleVar(type)) return type;
        type = this.getClassName(obj);
        return type;
    },

    /**
    * Deserialize an Array
    *
    * node (XML String) - The node to deserialize into an Array
    *
    *   returns the deserialized Array 
    */
    deserializeArray: function(node) {
        //console.log('deserializeArray');
        retObj = [];

        // Cycle through the array's TOP LEVEL children
        while (child = node.firstChild) {

            // delete child so it's children aren't recursed
            node.removeChild(node.firstChild);

            var nodeType = this.getNodeType(child);

            if (this.isSimpleVar(nodeType)) {
                retObj[retObj.length] = this.getTextContent(child);
            } else {
                var tmp = this.getTextContent(child);
                if (tmp.trim() != '') retObj[retObj.length] = this.deserial(child)
            }
        }
        return retObj;
    },

    /**
    * Deserialize a Function
    * node (XML String) - The node to deserialize into a Function
    *
    * returns the deserialized Function
    */
    deserializeFunction: function(func) {
        //console.log('deserializeFunction');
        var funcText = this.getTextContent(func);
        if (func && funcText) {
            // JSON hack to get functions converted correctly
            var tmp = JSON.decode("{\"tmp\":" + funcText.trim() + "}");
            return tmp.tmp;
        }
    },

    /**
    * Get the type attribute of an element if there is one,
    * otherwise return generic 'object'
    * 
    * NOTE: This function is used on the resulting serialized XML and not on
    *       any actual javascript object
    *
    * node (XML) - The node for which the type is to be found
    *
    * returns string of node attribute "type"
    */
    getNodeType: function(node) {
        //console.log('getNodeType');
        var nodeType = "object";

        if (node.attributes != null && node.attributes.length != 0) {
            var tmp = node.attributes.getNamedItem("type");
            if (tmp != null) nodeType = node.attributes.getNamedItem("type").nodeValue;
        }

        return nodeType;
    },

    /**
    * Get text content
    *
    * obj (XML Node) : the xml node to get the text content of
    * 
    * returns (string) of text content
    */
    getTextContent: function(obj) {
        //console.log('getTextContent');
        var funcText = (Browser.Engine.trident) ? obj.firstChild.text : obj.textContent;
        return funcText;
    }


});