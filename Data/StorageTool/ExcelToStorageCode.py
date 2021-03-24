# -*- coding: UTF-8 -*-
# ctype : 0 empty,1 string, 2 number, 3 date, 4 boolean, 5 error
import xlrd
import os
import sys
reload(sys)
sys.setdefaultencoding("utf-8")

codePath = u'Assets/FFF/Scripts/Framework/Storage/'
excelPath = u'read form parent of current path '
preClass = "Storage"
classes = []

def initConfig():
    global codePath
    global excelPath
    path = os.path.realpath(sys.path[0])

    os.chdir(path)
    projectPath = "" 
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
    excelPath = os.path.dirname(path)+"/Storage/"


def GetCodeTemplate(templatePath):
    fileInput = open(templatePath, 'r')
    codeT = fileInput.read()
    fileInput.close()
    return codeT


def CreateFold(path):
    isExists = os.path.exists(path)
    if not isExists:
        os.makedirs(path)


def IsSimpleData(pType):
    return {
        'int': True,
        'float': True,
        'string': True,
        'double': True,
        'ulong': True,
        'long': True,
        'bool': True,
    }.get(pType, False)


def sheetToCode(booksheet):
    namedata = booksheet.name.split('|')
    if len(namedata) == 2 and len(namedata[1]) > 0:
        className = preClass+namedata[1]
        classes.append(namedata[1])
        classFileName = className+".cs"
        codeT = GetCodeTemplate("StorageTemplate.cs")
        CreateFold(codePath)
        fileOutput = open(codePath+classFileName, 'w')
        # class name
        codeT = codeT.replace("StorageClassName", className)
        # SimpleProperty #ObjectProperty
        SimpleChangeEvent = ''
        SimpleProperty = ''
        ObjectProperty = ''
        SimpleChangeEventT = "public static string %s = \"%s\";\n\t\t"
        SimplePropertyT = GetCodeTemplate("SimplePropertyTemplate.cs")
        ObjectPropertyT = GetCodeTemplate("ObjectPropertyTemplate.cs")

        for row in xrange(1,booksheet.nrows):
            
            pDes = str(booksheet.cell(row,3).value)
            pName = str(booksheet.cell(row,0).value)
            if len(pName.strip()) <= 0 or pName.find("#") >= 0:
                continue
            pType = str(booksheet.cell(row,1).value)
            immediateUpdate = str(booksheet.cell(row,2).value)
            eventName = "%s_Change_%s"%(className,pName)

            SimpleChangeEventItem = SimpleChangeEventT%(eventName,eventName)
            SimpleChangeEvent += SimpleChangeEventItem

            if IsSimpleData(pType):
                SimplePropertyItem = SimplePropertyT.replace("#propertyName", pName).replace(
                    "#propertyType", pType).replace("#eventName", eventName).replace(
                    "#description", pDes).replace("#immediateValue","true" if(immediateUpdate == "true") else "false");
                SimpleProperty += SimplePropertyItem
            else:
                ObjectPropertyItem = ObjectPropertyT.replace("#propertyName", pName).replace(
                    "#propertyType", pType).replace("#eventName", eventName).replace("#description", pDes)
                ObjectProperty += ObjectPropertyItem

        codeT = codeT.replace("#SimpleChangeEvent", SimpleChangeEvent)
        codeT = codeT.replace("#SimpleProperty", SimpleProperty)
        codeT = codeT.replace("#ObjectProperty", ObjectProperty)
        fileOutput.write(codeT)
        fileOutput.close()


def ExcelToStorageCode(path):
    print "Excel to storage code path:"+path
    workbook = xlrd.open_workbook(path)
    print "There are {} sheets in the workbook".format(workbook.nsheets)
    for booksheet in workbook.sheets():
        sheetToCode(booksheet)


initConfig()
print "excelPath:"+excelPath
print "codePath:"+codePath
for root, dirs, files in os.walk(excelPath, topdown=False):
    for name in files:
        if name.find(".xlsx") > 0 and name.find("~") < 0:
            print "Excel Name:"+name
            ExcelToStorageCode(excelPath + "/" + name)

print("==================")
print("===== SUCESS =====")
print("==================")
            
