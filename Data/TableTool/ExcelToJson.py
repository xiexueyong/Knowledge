# -*- coding: UTF-8 -*-
# ctype : 0 empty,1 string, 2 number, 3 date, 4 boolean, 5 error
import xlrd
import os
import json
import string
import time

try:
    import sys
    reload(sys)
    sys.setdefaultencoding("utf-8")
except:
    import sys as sys


codePath = u'Assets/Framework/Table/TableClass/'
jsonPath = u'Assets/Export/Table/'
excelPath = u'read form parent of current path '
preClass = "Table"
classes = []
tapStr = '\t\t\t\t\t'
configTapStr = '\t\t\t\t'

argvList = sys.argv

def initConfig():
    global codePath
    global jsonPath
    global excelPath
    path = os.path.realpath(sys.path[0])

    projectPath = ""
    os.chdir(path)
    if os.path.exists("exportpath.config"):
        #读取配置路径
        fileInput = open("exportpath.config", 'r')
        projectPath = fileInput.read()
        fileInput.close()
    else:
        #读取相对路径
        projectPath = os.path.dirname(path)
        projectPath = os.path.dirname(projectPath)
        projectPath += "/Project/"
    codePath = projectPath+codePath
    jsonPath = projectPath+jsonPath
    excelPath = os.path.dirname(path)+"/Table/"
    print ("config:" + projectPath)



def GetCodeTemplate(templatePath):
    fileInput = open(templatePath, 'r')
    codeT = fileInput.read()
    fileInput.close()
    return codeT


def CreateFold(path):
    isExists = os.path.exists(path)
    if not isExists:
        os.makedirs(path)


def VType(pType):
    return {
        'arrayint': 'int[]',
        'arrayfloat': 'float[]',
        'arraystring': 'string[]',
        'jsonstring': 'string',
    }.get(pType, pType)
def GetValue(pType,value):
    if pType == "int":
        return int(value)
    elif pType == "string":
        return str(value)
    elif pType == "float":
        return float(value)
    elif pType == "bool":
        return bool(value)
    else:
        return str(value)

def setValueWraper(valueName, valueType):
    
    setValueItemT = "item.%s = itemData[\"%s\"];\n"
    setValueItemTint = "item.%s = Table.string2ArrayInt(itemData[\"%s\"]);\n"
    setValueItemTbool = "item.%s = Table.string2Bool(itemData[\"%s\"]);\n"
    setValueItemTInt2 = "item.%s = Table.string2Int2(itemData[\"%s\"]);\n"
    setValueItemTfloat = "item.%s = Table.string2ArrayFloat(itemData[\"%s\"]);\n"
    setValueItemTstring = "item.%s = Table.string2ArrayString(itemData[\"%s\"]);\n"
    setValueItemTTriangle_int_int_int = "item.%s = Table.string2Triangle_int_int_int(itemData[\"%s\"]);\n"
    setValueItemTDic_int_int = "item.%s = Table.string2Dic_int_int(itemData[\"%s\"]);\n"
    setValueItemTDic_string_string = "item.%s = Table.string2Dic_string_string(itemData[\"%s\"]);\n"
    setValueItemTDic_string_int = "item.%s = Table.string2Dic_string_int(itemData[\"%s\"]);\n"
    setValueItemTList_Triangle_int_int_int = "item.%s = Table.string2List_Triangle_int_int_int(itemData[\"%s\"]);\n"
    # print "======:"+valueType
    setValueItemTformat = {
        'arrayint': setValueItemTint,
        'Int2': setValueItemTInt2,
        'bool': setValueItemTbool,
        'arrayfloat': setValueItemTfloat,
        'arraystring': setValueItemTstring,
        'Dictionary<int,int>': setValueItemTDic_int_int,
        'Dictionary<string,string>': setValueItemTDic_string_string,
        'Dictionary<string,int>': setValueItemTDic_string_int,
        'Triangle<int,int,int>': setValueItemTTriangle_int_int_int,
        'List<Triangle<int,int,int>>': setValueItemTList_Triangle_int_int_int,
    }.get(valueType, setValueItemT)
    return setValueItemTformat % (valueName, valueName)


def sheetToConstCode(booksheet):
    namedata = booksheet.name.split('|')
    if len(namedata) == 3 and len(namedata[1]) > 0 and namedata[2] == 'config':
        className = preClass+namedata[1]
        # classes.append(namedata[1])
        classFileName = className+".cs"
        codeT = GetCodeTemplate("ConstTableTemplate.cs")
        CreateFold(codePath)
        fileOutput = open(codePath+classFileName, 'w')
        # class name
        codeT = codeT.replace("TableClassName", className)
        # DataFile
        codeT = codeT.replace("#DataFile", namedata[1])
        # properties #setValue
        properties = ''
        setvalues = ''
        propertyItemT = GetCodeTemplate("ConstPropertyTemplate.cs")

        idtype = str(booksheet.cell(2, 0).value)
        for row in range(2, booksheet.nrows):
            variateName = str(booksheet.cell(row, 0).value).strip()
            variateType = str(booksheet.cell(row, 1).value)
            variateRemark = str(booksheet.cell(row, 3).value)
            properties += propertyItemT.replace("#type",VType(variateType)).replace("#property", variateName).replace("#des",variateRemark)
            setvalues += configTapStr+setValueWraper(variateName, variateType)
            setvalues = setvalues.replace('item.',"")
        codeT = codeT.replace("#properties", properties)
        codeT = codeT.replace("#setValues", setvalues)
        codeT = codeT.replace("#idtype", idtype)
        fileOutput.write(codeT)
        fileOutput.close()

def sheetToFunctionCode(booksheet):
    namedata = booksheet.name.split('|')
    if len(namedata) == 3 and len(namedata[1]) > 0 and namedata[2] == 'Function':
        className = preClass+namedata[1]
        classFileName = className+".cs"
        codeT = GetCodeTemplate("FunctionClassTemplate.cs")
        CreateFold(codePath)
        fileOutput = open(codePath+classFileName, 'w')
        # class name
        codeT = codeT.replace("#TableClassName", className)
        # functions
        functions = ''
        functionItemT = GetCodeTemplate("FunctionItemTemplate.cs")
        annotionHeadT = GetCodeTemplate("AnnotionHeadTemplate.cs")
        annotionParamsItemT = GetCodeTemplate("AnnotionParamsItemTemplate.cs")


        idtype = str(booksheet.cell(2, 0).value)
        for row in range(2, booksheet.nrows):
            functionItem = ''
            dataItem = ''
            arguments = ''
            annotationParamsItems = ''

            funName = str(booksheet.cell(row, 0).value).strip()
            eventName = str(booksheet.cell(row, 1).value).strip()
            annotation  = str(booksheet.cell(row, 2).value).strip()
            annotationHead = annotionHeadT.replace("#annotation",annotation);
            for col in range(5,booksheet.ncols,3):
                paramdes = str(booksheet.cell(row, col-2).value).strip()
                argType = str(booksheet.cell(row, col-1).value).strip()
                argName = str(booksheet.cell(row, col).value).strip()

                if argType == '' or argName == '':
                    break
                arguments += argType+" "+argName+","
                if argType.find("Dictionary") >= 0:
                    dataItem += "\t\t\tAddDicAsJsonToDic(dic, \"{0}\", {1});\n".format(argName,argName)
                else:
                    dataItem += "\t\t\tdic.Add(\"{0}\", {1}.ToString());\n".format(argName,argName)
                annotationParamsItems += "\n"+annotionParamsItemT.replace("#paramName",argName).replace("#des",paramdes)

            functionItem = functionItemT.replace("#FunName",funName).replace("#eventName", eventName).replace("#annotation",annotationHead+annotationParamsItems)
            functionItem = functionItem.replace("#arguments",arguments[0:-1])
            functionItem = functionItem.replace("#dataItem",dataItem)
            functions += functionItem

        codeT = codeT.replace("#functions", functions)
        fileOutput.write(codeT)
        fileOutput.close()


def sheetToConstJson(booksheet):
    namedata = booksheet.name.split('|')
    if len(namedata) == 3 and len(namedata[1]) > 0 and namedata[2] == 'config':
        jsonfileName = namedata[1]+".json"
        CreateFold(jsonPath)
        fileOutput = open(jsonPath+jsonfileName, 'w')
        writeData = json.loads("{}")

        for row in range(2,booksheet.nrows):
            variateName = str(booksheet.cell(row, 0).value)
            variateType = str(booksheet.cell(row, 1).value)
            variateValue = GetValue(variateType,booksheet.cell(row, 2).value)
            writeData[variateName] = variateValue;

        finalData = json.dumps(writeData)
        fileOutput.write(finalData)
        fileOutput.close()

def sheetToTableJson(booksheet):
    namedata = booksheet.name.split('|')
    if len(namedata) == 2 and len(namedata[1]) > 0:
        jsonfileName = namedata[1]+".json"
        fileOutput = open(jsonPath+jsonfileName, 'w')
        CreateFold(jsonPath)
        writeData = "["
        for row in range(booksheet.nrows):
            if row > 2:
                #id值未填则忽略
                idValue = str(booksheet.cell(row, 0).value)
                if len(idValue.strip()) <= 0:
                    continue
                writeData = writeData + "{"
                for col in range(booksheet.ncols):
                    key = str(booksheet.cell(1, col).value)
                    #属性名有!则忽略
                    if len(key.strip()) <= 0 or key.find("!") >= 0:
                        continue
                    
                    valueType = str(booksheet.cell(2, col).value)
                    celltype = booksheet.cell(2, col).ctype
                    # print "celltype:%d"%celltype
                    if valueType == "int":
                        try:
                            value = int(booksheet.cell(row, col).value)
                        except Exception as e:
                            value = 0
                    elif valueType == "float":
                        try:
                            value = float(booksheet.cell(row, col).value)
                        except Exception as e:
                            value = 0.0
                    else:
                        ctype = booksheet.cell(row, col).ctype  # 表格的数据类型
                        value = booksheet.cell(row, col).value
                        if ctype == 2 and value % 1 == 0.0:  # ctype为2且为浮点
                            value = str(int(value))  # 浮点转成整型
                        else:
                            value = str(value)
                        value = value.replace("\"", "\\\"")
                        value = '"'+str(value)+'"'

                    writeData = writeData + '"' + key + '":'+str(value)+","
                writeData = writeData[0:-1]
                writeData += "},\n"
        writeData = writeData[0:-2]
        writeData += "]"
        #writeData = writeData.replace("'", "\"")
        fileOutput.write(writeData)
        fileOutput.close()


def sheetToTableCode(booksheet):
    namedata = booksheet.name.split('|')
    if len(namedata) == 2 and len(namedata[1]) > 0:
        className = preClass+namedata[1]
        # classes.append(namedata[1])
        classFileName = className+".cs"
        codeT = GetCodeTemplate("TableTemplate.cs")
        CreateFold(codePath)
        fileOutput = open(codePath+classFileName, 'w')
        # class name
        codeT = codeT.replace("TableClassName", className)
        # DataFile
        codeT = codeT.replace("#DataFile", namedata[1])
        # properties #setValue
        properties = ''
        setvalues = ''
        propertyItemT = "\t\tpublic %s %s { private set; get; }\n"

        idtype = str(booksheet.cell(2, 0).value)
        for col in range(booksheet.ncols):
            pDes = str(booksheet.cell(0, col).value)
            pName = str(booksheet.cell(1, col).value).strip()
            if len(pName.strip()) <= 0 or pName.find("!") >= 0:
                continue
            pType = str(booksheet.cell(2, col).value)
            properties += "\t\t//%s\n"%pDes
            properties += propertyItemT % (VType(pType), pName)
            setvalues += tapStr+setValueWraper(pName, pType)
        codeT = codeT.replace("#properties", properties)
        codeT = codeT.replace("#setValues", setvalues)
        codeT = codeT.replace("#idtype", idtype)
        fileOutput.write(codeT)
        fileOutput.close()


def ExcelToJson(excelFileName):
    path = excelPath + "/" + excelFileName
    fileName = name.split('.')[0]
    
    t0 = time.clock()
    workbook = xlrd.open_workbook(path)
   

    for booksheetName in workbook.sheet_names():
        namedata = booksheetName.split('|')
        if len(namedata) >= 2:
            if namedata[-1] != 'Function' and namedata[1].find("__") < 0:
                classes.append(namedata[1])
        toExport = False
        if len(argvList) > 1:
            if fileName in argvList:
                toExport = True
        else:
            toExport = True
        if toExport:
            if len(namedata) >=2:
                # print("Excel to json path:" + path)
                booksheet = workbook.sheet_by_name(booksheetName)
                # print("There are {} sheets in the workbook".format(workbook.nsheets))
                if len(namedata) == 3 and len(namedata[1]) > 0 and namedata[2] == 'config':
                    if namedata[1].find("__") < 0:
                        sheetToConstCode(booksheet)
                    sheetToConstJson(booksheet)
                elif len(namedata) == 3 and len(namedata[1]) > 0 and namedata[2] == 'Function':
                    sheetToFunctionCode(booksheet)
                else:
                    if namedata[1].find("__") < 0:
                        sheetToTableCode(booksheet)
                    sheetToTableJson(booksheet)
                outputNameStr = fileName +" ==> " +namedata[1]
                outputNameStr += (40 - len(outputNameStr)) * " "
                print((outputNameStr+"========> Time:{0}{1}").format(round(time.clock() - t0, 3), "s"))


def GenerateTableManager():
    properties = ''
    clearTables = ''
    initTables = ''
    #itemT = "       private %s%s _%s;\n"+"     public %s%s %s\n"+"       {get{\n"+"        if (_%s == null){_%s = new %s%s();}"+"\n            return _%s;} }\n\n"
    itemT = "\tpublic static  %s%s %s = new  %s%s();\n\n"
    clearTableItemT = "\t\t%s.Clear();\n"
    initTableItemT = "\t\t\t%s.Init();\n"
    for className in classes:
        properties += itemT % (preClass, className,
                               className, preClass, className)
        clearTables += clearTableItemT % (className)
        initTables += initTableItemT % (className)
    managerT = GetCodeTemplate("TableManagerTemplate.cs")
    managerT = managerT.replace("#properties", properties).replace("#clearTable", clearTables).replace("#initTable", initTables)
    fileOutput = open(codePath+"Table.cs", 'w')
    fileOutput.write(managerT)
    fileOutput.close()


initConfig()
print("excelPath:"+excelPath)
print("codePath:"+codePath)
print("jsonPath:"+jsonPath)
totolTime1 = time.clock();
for root, dirs, files in os.walk(excelPath, topdown=False):
    for name in files:
        if name.find(".xlsx") > 0 and name.find("~") < 0:
            ExcelToJson(name)

GenerateTableManager()


print("====================================")
print("============== SUCESS ==============")
print(("========= TotalTime: {0}{1}").format(round(time.clock() - totolTime1, 2), "s ========="))
print("====================================")
