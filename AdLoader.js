var delay = 10000;
//var adsPresent = 2;
var adIndex = 0;
var maxAttempts = 10;
var active = true;
var adIndices = [];
var wideIndices = [];
var iterations = 0;
var url = "http://localhost:5000";

window.onblur = function() { active = false; };
window.onfocus = function() { active = true; };

function AdDelay(timeout)
{
    return new Promise((resolve) => setTimeout(resolve, timeout));
}

async function Fetcher(urlBase, linkBase, availableAds)
{
    var index = 0;
    while(index < maxAttempts)
    {
        /*if(!document.getElementById(urlBase + index))
        {
            break;
        }*/
        if(active) //if(!document.hidden)
        {
            if(document.getElementById(urlBase + index ) && document.getElementById(linkBase + index))
            {
                adIndex = adIndex % availableAds.length;
                await fetch(url + "/api/AD/getAD?adID=" + availableAds[adIndex] + "&count=true"/*, {headers : {"ngrok-skip-browser-warning" : "true"}}*/).then((response) =>{
                    return response.json();
                }).then((data) =>{
                    var dimensions = iterations <= 0 ? document.getElementById(urlBase + index).src.indexOf("wide") > -1 ? "true" : "false" : wideIndices[index];
                    document.getElementById(urlBase + index).src = url + "/api/AD/getImage?adID=" + adIndex + "&wide=" + dimensions;
                    document.getElementById(linkBase + index).href = url + "/api/AD/adClicked?adID=" + adIndex;
                    wideIndices[index] = dimensions;
                });

                adIndex++;
            }
        }

        index++;
    }

    iterations++;
    await AdDelay(delay);
    Fetcher(urlBase, linkBase, availableAds);
}

fetch(url + "/api/AD/availableAds").then(
(response) => { return response.json(); }).then(
    (data) => {
        adIndices = data;
        Fetcher("url", "link", adIndices);
});
/*var delay = 10000;
var adsPresent = 2;
var adIndex = 0;

function AdDelay(timeout)
{
    return new Promise((resolve) => setTimeout(resolve, timeout));
}

async function Fetcher(urlBase, linkBase, availableAds)
{
    for(var i = 0; i < adsPresent; i++)
    {
        adIndex = adIndex % availableAds.length;
        await fetch("http://localhost:5000/api/AD/getAD?adID=" + availableAds[adIndex] + "&count=true").then((response) =>{
            return response.json();
        }).then((data) =>{
            document.getElementById(urlBase + i).src = "http://localhost:5000/api/AD/getImage?adID=" + adIndex;
            document.getElementById(linkBase + i).href = "http://localhost:5000/api/AD/adClicked?adID=" + adIndex;
        })

        adIndex++;
    }

    await AdDelay(delay);
    Fetcher(urlBase, linkBase, availableAds);
}

Fetcher("url", "link", [0, 1, 2, 3, 4]);*/