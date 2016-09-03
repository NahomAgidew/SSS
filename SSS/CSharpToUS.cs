/*
 * User: Nahom
 * Date: 6/27/2016
 * Time: 7:31 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System.IO;
using System.Text;

namespace SSS
{
  public class CSharpToUS
  {
    private StringBuilder compiledCode = new StringBuilder();
    private StringBuilder builder = new StringBuilder();
    private string[] rawcode;

    public CSharpToUS(string CSharpFilePath, string Destination)
    {
      this.Main(CSharpFilePath, Destination);
    }

    private void Main(string _CSharpFilePath, string _Destination)
    {
      this.rawcode = File.ReadAllLines(_CSharpFilePath);
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
      string[] strArray1 = new string[19]
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
      string[] strArray2 = new string[19]
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
      this.builder.Replace("using", "import");
      this.builder.Replace("void", "function");
      if (this.builder.ToString().Contains("class") && this.builder.ToString().Contains(":"))
        this.builder.Replace(":", "extends");
      if (this.builder.ToString().Contains("[") && this.builder.ToString().Contains("(") && (this.builder.ToString().Contains(")") && this.builder.ToString().Contains("]")))
      {
        this.builder.Replace("[", "@");
        this.builder.Replace("]", "");
      }
      else if (this.builder.ToString().Contains("[") && this.builder.ToString().Contains("]") && (!this.builder.ToString().Contains("(") && !this.builder.ToString().Contains(")")) && !this.builder.ToString().Contains(";"))
      {
        this.builder.Replace("[", "@");
        this.builder.Replace("]", "");
      }
      if (this.builder.ToString().Contains("function") && this.builder.ToString().Contains("(") && this.builder.ToString().Contains(")"))
      {
        int startIndex = this.builder.ToString().IndexOf("(") + 1;
        int length = this.builder.Length - 1 - startIndex;
        if (this.builder.ToString().Substring(startIndex, length) != "" && !this.builder.ToString().Contains(","))
        {
          StringBuilder stringBuilder = new StringBuilder(this.builder.ToString().Substring(startIndex, length));
          foreach (string oldValue1 in strArray2)
          {
            if (stringBuilder.ToString().Contains(oldValue1))
            {
              string oldValue2 = stringBuilder.ToString().Replace(oldValue1, "");
              string newValue = stringBuilder.ToString().Replace(oldValue1, "").Trim(' ');
              this.builder.Replace(oldValue1, newValue);
              this.builder.Insert(this.builder.ToString().IndexOf(newValue) + newValue.Length, ":" + oldValue1);
              this.builder.Replace(oldValue2, "");
            }
          }
        }
      }
      bool flag1 = true;
      foreach (string str in strArray1)
      {
        bool flag2 = this.builder.ToString().Contains(str) && this.builder.ToString().Contains("(") && (this.builder.ToString().Contains(")") && !this.builder.ToString().Contains(";"));
        if (flag2 && this.builder.ToString().IndexOf("(") > this.builder.ToString().IndexOf(str) && !flag1)
          this.builder.Replace(str, "function");
        if (!flag2)
        {
          bool flag3 = false;
          if (!this.builder.ToString().Contains("new"))
            flag3 = false;
          if (this.builder.ToString().Contains("new") && this.builder.ToString().Contains("(") && (this.builder.ToString().Contains(")") && this.builder.ToString().Contains(str)) && (this.builder.ToString().LastIndexOf(str) > this.builder.ToString().IndexOf("new") && this.builder.ToString().IndexOf(str) < this.builder.ToString().LastIndexOf(str)))
            flag3 = true;
          if (flag3)
            this.builder.Replace(str, "var", this.builder.ToString().IndexOf(str), str.Length);
          if (!flag3 && this.builder.ToString().IndexOf("(") > this.builder.Length - this.builder.ToString().IndexOf(str))
            this.builder.Replace(str, "var");
          if (this.builder.ToString().Contains(str) && this.builder.ToString().Contains("=") && !flag3)
          {
            if (!this.builder.ToString().Contains("."))
            {
              if (str != "bool" && str != "string")
              {
                this.builder.Replace(str, "var");
                this.builder.Insert(this.builder.ToString().IndexOf("=") - 1, " :");
                this.builder.Insert(this.builder.ToString().IndexOf("="), str);
              }
              if (str == "bool")
              {
                this.builder.Replace(str, "var");
                this.builder.Insert(this.builder.ToString().IndexOf("=") - 1, " :");
                this.builder.Insert(this.builder.ToString().IndexOf("="), "boolean");
              }
              if (str == "string")
              {
                this.builder.Replace(str, "var");
                this.builder.Insert(this.builder.ToString().IndexOf("=") - 1, " :");
                this.builder.Insert(this.builder.ToString().IndexOf("="), "String");
              }
            }
            if (this.builder.ToString().Contains(".") && this.builder.ToString().IndexOf("=") < this.builder.ToString().IndexOf("."))
              this.builder.Replace(str, "var");
          }
          if (this.builder.ToString().Contains(str) && !this.builder.ToString().Contains("(") && (!this.builder.ToString().Contains(")") && !flag3) && (this.builder.ToString().Contains(str) && this.builder.ToString().Contains(";")))
          {
            int num = (int) this.builder[this.builder.ToString().LastIndexOf(str) - 1];
            if (!this.builder.ToString().Contains(".") && !this.builder.ToString().Contains("="))
            {
              if (str != "bool" && str != "string")
              {
                this.builder.Replace(str, "var", this.builder.ToString().IndexOf(str), str.Length);
                this.builder.Replace(";", ":");
                this.builder.Append(str);
                this.builder.Append(";");
              }
              if (str == "bool")
              {
                this.builder.Replace(str, "var", this.builder.ToString().IndexOf(str), str.Length);
                this.builder.Replace(";", ":");
                this.builder.Append("boolean");
                this.builder.Append(";");
              }
              if (str == "string")
              {
                this.builder.Replace(str, "var", this.builder.ToString().IndexOf(str), str.Length);
                this.builder.Replace(";", ":");
                this.builder.Append("String");
                this.builder.Append(";");
              }
            }
          }
          if (this.builder.ToString().Contains("var") && this.builder.ToString().Contains("(") && (this.builder.ToString().Contains(")") && this.builder.ToString().Contains(";")))
          {
            if (this.builder.ToString().IndexOf("=") > this.builder.ToString().IndexOf("."))
              this.builder.Replace("var", str);
            else if (!this.builder.ToString().Contains("."))
              this.builder.Replace("var", str);
          }
        }
      }
      if (this.builder.ToString().Contains("<") && this.builder.ToString().Contains(">") && (this.builder.ToString().Contains("(") && this.builder.ToString().Contains(")")))
      {
        this.builder.Replace("(", "");
        this.builder.Replace(")", "");
        this.builder.Replace("<", "(");
        this.builder.Replace(">", ")");
      }
      this.compiledCode.AppendLine(this.builder.ToString());
    }
  }
}

