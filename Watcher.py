import bs4;
import requests;
import argparse;
import os;

def InitializeArguments() : 
    parser = argparse.ArgumentParser("Night Crawler");
    parser.add_argument("-u", "--url", help = "Target URL;");
    parser.add_argument("-r", "--recurse", help = "Recursive Parsing;", default = True);
    parser.add_argument("-d", "--directory", help = "Output Directory;", default = os.path.join(os.getcwd(), "Watcher_Dump"));

    return parser.parse_args();

def GetBaseURL(url : str) : 
    divider = url.split(".");
    remainder = divider[len(divider) - 1];
    base = "";

    for index in range(len(divider) - 1) : 
        base += "%s." % divider[index];

    return base + remainder.split("/")[0];

arguments = InitializeArguments();

response = requests.get(arguments.url);
baseURL = GetBaseURL(arguments.url);

websitePath = os.path.join(arguments.directory, "Watcher.html");
if(not os.path.isfile(websitePath)) : 
    with open(websitePath, "wb") as site : 
        site.write(response.content);

souper = bs4.BeautifulSoup(response.text, features = "html.parser");
#print(souper.prettify());


scripts = souper.find_all("script");
links = souper.find_all("a");
images = souper.find_all("img");

#tags = ["script", "a", "img"];
#contents = ["src", "href", "src"];
tags = ["img", "script"];
contents = ["src", "src"];

elements = [souper.find_all(tag) for tag in tags];

for index in range(len(elements)) : 
    for element in elements[index] :
        content = element.get(contents[index]);
        if(content) : 
            if(content.startswith("/")) : 
                path = str(arguments.directory);
                availableFolders = content.split("/");
                for folderIndex in range(len(availableFolders) - 1) : 
                    path = os.path.join(path, availableFolders[folderIndex]);
                    if(not os.path.exists(path)) : 
                        os.mkdir(path);
                filename = availableFolders[len(availableFolders) - 1];
                print(f"{baseURL + content} : {filename};");
                try :
                    filteredFilename = filename.split("?")[0];
                    if(not os.path.isfile(os.path.join(path, filteredFilename))) : 
                        file = requests.get(baseURL + content);
                        with open(os.path.join(path, filteredFilename), "wb") as writeFile : 
                            writeFile.write(file.content);
                except : 
                    print("[-] Error Downloading the File %s;" % filename);
                #print("{folder} : {exists}, {_content}".format(folder = path, exists = os.path.exists(path), _content = content));
        #print("%s, %i : %s" % (tags[index], index, element.get(contents[index]))); 

