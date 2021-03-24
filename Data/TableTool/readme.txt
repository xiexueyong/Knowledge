1.配置python Path环境变量.默认为C:\Python27

2.配置python easy_install环境变量，默认为C:\Python27\Scripts

3.安装pip，安装命令为easy_install install pip

4.安装xlrd，安装命令为pip install xlrd

5.复制exportpathT.config，并改名为exportpath.config，在内部填上项目的路径 ../WordBuild/
6.执行ExcelToJson.py脚本即可



//常规excel
支持的数据类型:
string
int
Int2
float
arrayfloat
arrayint
arraystring
Dictionary<int,int>
Jsonstring //可以处理导出json文件不包含 转义反斜杠
支持数据类型扩展

//配置表excel
详情配置请看Const.excel
