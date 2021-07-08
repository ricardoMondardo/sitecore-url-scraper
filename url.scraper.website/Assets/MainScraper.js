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
    let url = document.getElementById('input-url').value;

    try {
        let resp = await fetch(`/api/UrlScraperApi/Scraper/${url}`, requestOptions)
        let data = await resp.json();

        let xValues = data.ListWords.map((x) => { return x.word;})
        let yValues = data.ListWords.map((x) => { return x.count })

        let MaxValue = yValues[0];
        let MinValue = yValues[yValues.length-1];

        console.log(MaxValue, MinValue);

        new Chart("myChart", {
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

        const GridImgs = document.getElementById('grid-imgs');
        GridImgs.innerHTML = "";
        for (let [key, value] of Object.entries(data.ListImages)) {
            GridImgs.innerHTML +=
                `<img src=${value}>`;
            console.log(value);
        }

        console.log('Data', data);
               

    } catch (error) {
        console.error(error);
    }
    
}

function getInfo() {
    CallApi();
}



