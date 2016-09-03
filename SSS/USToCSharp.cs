/*
 * User: Nahom
 * Date: 6/27/2016
 * Time: 7:30 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System.IO;
using System.Text;

namespace SSS
{
  public class USToCSharp
  {
    private StringBuilder compiledCode = new StringBuilder();
    private StringBuilder builder = new StringBuilder();
    private string[] rawcode;

    public USToCSharp(string JavascriptFilePath, string Destination)
    {
      this.Main(JavascriptFilePath, Destination);
    }

    private void Main(string _JavascriptFilePath, string _Destination)
    {
      this.rawcode = File.ReadAllLines(_JavascriptFilePath);
      for (int index = 0; index < this.rawcode.Length; ++index)
      {
        this.builder.Append(this.rawcode[index]);
        this.Compile();
        this.builder.Remove(0, this.builder.Length);
      }
      File.WriteAllText(_Destination, this.compiledCode.ToString());
    }

    private void Compile()
    {
      string[] strArray = new string[19]
      {
        "Collider",
        "Collision",
        "string",
        "int",
        "double",
        "float",
        "Vector3",
        "Vector2",
        "Light",
        "Texture2D",
        "Button",
        "Image",
        "Transform",
        "Quaternion",
        "bool",
        "GameObject",
        "Animator",
        "AudioClip",
        "AnimationClip"
      };
      this.builder.Replace("import", "using");
      this.builder.Replace("function", "void");
      this.builder.Replace("String", "string");
      if (this.builder.ToString().Contains("class") && this.builder.ToString().Contains("extends"))
        this.builder.Replace("extends", ":");
      if (this.builder.ToString().Contains("@") && this.builder.ToString().Contains("(") && this.builder.ToString().Contains(")"))
      {
        if (this.builder.ToString().Contains("script"))
          this.builder.Replace("script", "");
        this.builder.Replace("@", "[");
        this.builder.Append("]");
      }
      else if (this.builder.ToString().Contains("@") && !this.builder.ToString().Contains(";"))
      {
        this.builder.Replace("@", "[");
        this.builder.Append("]");
      }
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string oldValue in strArray)
      {
        if (this.builder.ToString().Contains(":") && this.builder.ToString().Contains(oldValue) && this.builder.ToString().Contains("var"))
        {
          string newValue = this.builder.ToString().Substring(this.builder.ToString().IndexOf("var") + 3, this.builder.Length - this.builder.ToString().IndexOf(":") - 1);
          this.builder.Remove(this.builder.ToString().IndexOf(oldValue), oldValue.Length);
          this.builder.Replace(oldValue, newValue);
          this.builder.Insert(this.builder.ToString().IndexOf("var") + 3, " ");
          this.builder.Insert(this.builder.ToString().IndexOf("var") + 4, oldValue);
          this.builder.Replace(":", "");
          this.builder.Replace("var", "");
        }
      }
      if (this.builder.ToString().Contains("void") && this.builder.ToString().Contains("(") && this.builder.ToString().Contains(")"))
      {
        int startIndex = this.builder.ToString().IndexOf("(") + 1;
        int length1 = this.builder.Length - 1 - startIndex;
        if (this.builder.ToString().Substring(startIndex, length1) != "" && !this.builder.ToString().Contains(","))
        {
          foreach (string oldValue in strArray)
          {
            if (this.builder.ToString().Contains(oldValue))
            {
              this.builder.ToString().Substring(this.builder.ToString().IndexOf(oldValue), oldValue.Length);
              int length2 = this.builder.ToString().IndexOf(":") - (this.builder.ToString().IndexOf("(") + 1);
              string newValue = this.builder.ToString().Substring(this.builder.ToString().IndexOf("(") + 1, length2);
              this.builder.Replace(oldValue, newValue);
              this.builder.Remove(this.builder.ToString().IndexOf("(") + 1, length2);
              this.builder.Insert(this.builder.ToString().IndexOf("(") + 1, oldValue);
              this.builder.Replace(":", "");
            }
          }
        }
      }
      if (this.builder.ToString().Contains("GetComponent") && this.builder.ToString().Contains("(") && (this.builder.ToString().Contains(")") && this.builder.ToString().IndexOf("GetComponent") < this.builder.ToString().IndexOf("(")) && this.builder.ToString().IndexOf("GetComponent") < this.builder.ToString().IndexOf(")"))
      {
        this.builder.Replace("(", "<");
        this.builder.Replace(")", ">");
        this.builder.Insert(this.builder.ToString().IndexOf(">") + 1, "(");
        this.builder.Insert(this.builder.ToString().IndexOf(">") + 2, ")");
      }
      this.compiledCode.AppendLine(this.builder.ToString());
    }
  }
}
