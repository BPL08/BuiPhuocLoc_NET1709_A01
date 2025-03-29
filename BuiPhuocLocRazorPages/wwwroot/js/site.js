const connection = new signalR.HubConnectionBuilder()
    .withUrl("/newsHub")
    .build();

// Start the connection
connection.start().then(() => {
    console.log("SignalR connected.");
}).catch(err => console.error("SignalR Error:", err));

// Listen for "LoadNews" message from SignalR Hub
connection.on("LoadNews", () => {
    // Reloading the entire page, could be optimized to refresh only necessary parts
    location.reload();
});

// Listen for "ReceiveNewsUpdate" to handle news updates
connection.on("ReceiveNewsUpdate", (message) => {
    console.log("News Update:", message);
    // Here you can update specific parts of the page if needed
    // For example, you could update a news list or news article section without reloading
});
