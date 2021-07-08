console.log("Hi")

async function CallApi() {

    const requestOptions = {
        method: "GET",
        mode: "cors",
        cache: "no-cache",
        credentials: "same-origin",
        headers: {
            "Content-Type": "application/json"
        },
        redirect: "follow",
        referrer: "no-referrer"
    };

    const chart = document.getElementById('chart-words');
    const msgCall = document.getElementById('msg-call');
    const GridImgs = document.getElementById('grid-imgs');

    let url = document.getElementById('input-url').value;


    url = url.replace("https", "").replace("http", "");
    url = url.replace("://", "").replace(":/", "");

    chart.style.display = "none";

    GridImgs.innerHTML = "";
    msgCall.innerHTML = "Loading..."

    try {

        let resp = await fetch(`/api/UrlScraperApi/Scraper/${url}`, requestOptions)
        
        if (resp.ok) {

            let data = await resp.json();
            if (data) {

                msgCall.innerHTML = "";

                let xValues = data.ListWords.map((x) => { return x.word; })
                let yValues = data.ListWords.map((x) => { return x.count })

                let MaxValue = yValues[0];
                let MinValue = yValues[yValues.length - 1];

                chart.style.display = "block";

                new Chart("chart-words", {
                    type: "line",
                    data: {
                        labels: xValues,
                        datasets: [{
                            fill: false,
                            lineTension: 0,
                            backgroundColor: "rgba(0,0,255,1.0)",
                            borderColor: "rgba(0,0,255,0.1)",
                            data: yValues
                        }]
                    },
                    options: {
                        legend: { display: false },
                        scales: {
                            yAxes: [{ ticks: { min: MinValue, max: MaxValue } }],
                        }
                    }
                });
                                
                for (let [key, value] of Object.entries(data.ListImages)) {
                    GridImgs.innerHTML += `<img src=${value}>`;
                    console.log(value);
                }

            } else {
                console.log(resp);
                msgCall.innerHTML = "Cannot find the website document."
            }
        } else {
            console.log(resp);
            msgCall.innerHTML = "Please, check the address and try again."
        }
               
    } catch (error) {
        console.log(error);
        msgCall.innerHTML = "Please, check the address and try again."
    }
    
}

function getInfo() {
    CallApi();
}



