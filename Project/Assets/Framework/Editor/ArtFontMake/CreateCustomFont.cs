//------------------------------------------------------------------------------
// <summary>
// 描述： 
// 创建者：
// 创建时间：
// 修改者：
// 修改时间：
// </summary>
//------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;
using System;
using UnityEngine.Networking;
public class CreateCustomFont : Editor
{
    [MenuItem("DevHelper/CreateCustomFont")]
    static void CreateConstumFontFunction()
    {
        UnityEngine.Object obj = Selection.activeObject;
        string fullName = "";  //完整文件名
        string fontFileName = "";     //生成字体资源的文件名
        string dicName = "";  //目录名
        int textWidth = 0;
        int textHight = 0;
        if (obj != null)
        {
            fullName = AssetDatabase.GetAssetPath(obj);
            if (fullName.IndexOf(".fnt") == -1)
            {
                EditorUtility.DisplayDialog("异常", "不是字体文件，请选择字体文件", "确定");
                return;
            }
            dicName = Path.GetDirectoryName(fullName) + "/";
            fontFileName = Path.GetFileName(fullName).Replace(".fnt", ".fontsettings");
            StreamReader reader = new StreamReader(File.OpenRead(fullName));
            List<CharacterInfo> infos = new List<CharacterInfo>();
            string line = reader.ReadLine();
            Regex regex = new Regex(@"char id=(?<id>\d+)\s+x=(?<x>\d+)\s+y=(?<y>\d+)\s+width=(?<width>\d+)\s+height=(?<height>\d+)\s+xoffset=(?<xoffset>\d+)\s+yoffset=(?<yoffset>\d+)\s+xadvance=(?<xadvance>\d+)\s+");
            while (line != null)
            {
                if (line.StartsWith("char id="))
                {
                    Match match = regex.Match(line);
                    if (match != Match.Empty)
                    {
                        var id = Convert.ToInt32(match.Groups["id"].Value);
                        var x = Convert.ToInt32(match.Groups["x"].Value);
                        var y = Convert.ToInt32(match.Groups["y"].Value);
                        var width = Convert.ToInt32(match.Groups["width"].Value);
                        var height = Convert.ToInt32(match.Groups["height"].Value);
                        var xoffset = Convert.ToInt32(match.Groups["xoffset"].Value);
                        var yoffset = Convert.ToInt32(match.Groups["yoffset"].Value);
                        var xadvance = Convert.ToInt32(match.Groups["xadvance"].Value);

                        CharacterInfo info = new CharacterInfo();
                        info.index = id;
                        float uvx = 1f * x / textWidth;
                        float uvy = 1 - 1f * y / textHight;
                        float uvw = 1f * width / textWidth;
                        float uvh = -1f * height / textHight;
                        //info.uvBottomLeft = new Vector2(uvx,uvy);
                        //info.uvBottomRight = new Vector2(uvx +uvw,uvy);
                        //info.uvTopLeft = new Vector2(uvx ,uvy+uvh);
                        //info.uvTopRight = new Vector2(uvx+uvw,uvy+uvh);
                        info.uv = new Rect(uvx, uvy, uvw, uvh);
                        //info.minX = xoffset;
                        //info.minY = yoffset;
                        ////info.maxX =  width;
                        ////info.maxY = -height;
                        //info.glyphWidth = width;
                        //info.glyphHeight = -height;
                        info.vert = new Rect(xoffset, yoffset, width, height);
                        info.advance = xadvance;
                        infos.Add(info);
                    }
                }
                else if (line.IndexOf("scaleW=") != -1)
                {
                    Regex regex2 = new Regex(@"common lineHeight=(?<lineHeight>\d+)\s+.*scaleW=(?<scaleW>\d+)\s+scaleH=(?<scaleH>\d+)");
                    Match match2 = regex2.Match(line);
                    if (match2 != Match.Empty)
                    {
                        textWidth = Convert.ToInt32(match2.Groups["scaleW"].Value);
                        textHight = Convert.ToInt32(match2.Groups["scaleH"].Value);
                    }
                }
                line = reader.ReadLine();
            }
            reader.Close();
            Texture2D tex = null;
            {
                string texturePath = dicName + (fontFileName.Replace(".fontsettings", "") + "_0") + ".png";
                tex = AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture2D)) as Texture2D;
                AssetDatabase.MoveAsset(texturePath, texturePath.Replace("_0", ""));
            }
            if (tex == null)
            {
                EditorUtility.DisplayDialog("异常", "图片未找到，请检查资源名！", "确定");
                return;
            }
            Font customFont = new Font();
            AssetDatabase.CreateAsset(customFont, dicName + fontFileName);
            customFont.characterInfo = infos.ToArray();

            Material mat = null;
            {
                mat = new Material(Shader.Find("GUI/Text Shader"));
                mat.SetTexture("_MainTex", tex);
                AssetDatabase.CreateAsset(mat, fullName.Replace(".fnt", ".mat"));
            }
            customFont.material = mat;
            EditorUtility.SetDirty(customFont);
            EditorUtility.SetDirty(mat);
            AssetDatabase.SaveAssets();

            AssetDatabase.ExportPackage(dicName + fontFileName, "temp.unitypackage");
            AssetDatabase.DeleteAsset(dicName + fontFileName);
            AssetDatabase.ImportPackage("temp.unitypackage", false);
            AssetDatabase.Refresh();
        }
        else
        {
            EditorUtility.DisplayDialog("异常", "未选中任何资源！", "确定");
        }
    }



}
