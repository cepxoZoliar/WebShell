const outputElement = document.getElementById("output");
const inputElement = document.getElementById("command-input");
const runButton = document.getElementById("run-button");

let commandHistory = [];
let historyIndex = 0;

async function loadHistory() {
    const response = await fetch("/api/Commands/history");
    if (response.ok) {
        commandHistory = await response.json();
    }
    historyIndex = commandHistory.length;
}

loadHistory();

async function runCommand() {
    const command = inputElement.value;
    if (command.trim() === "") {
        return;
    }
    
    const response = await fetch("/api/commands", {
        method: "POST",
        headers: {"Content-Type": "application/json"},
        body: JSON.stringify({command: command}),
    });
    
    if (!response.ok) {
        outputElement.textContent += "Ошибка запроса " + response.status + "\n";
        return;
    }
    
    const result = await response.json();
    
    outputElement.textContent += "$ " + command + "\n";
    outputElement.textContent += result.output;
    outputElement.textContent += result.error;
    
    commandHistory.push(command);
    historyIndex = commandHistory.length;
    
    inputElement.value = "";
    outputElement.scrollTop = outputElement.scrollHeight;
    inputElement.focus();
}

runButton.addEventListener("click", runCommand);

inputElement.addEventListener("keydown", event => {
    if (event.key === "Enter") {
        runCommand();
    } else if (event.key === "ArrowUp") {
        event.preventDefault();
        if (historyIndex > 0) {
            historyIndex--;
            inputElement.value = commandHistory[historyIndex];
        }
    } else if (event.key === "ArrowDown") {
        event.preventDefault();
        if (historyIndex < commandHistory.length - 1) {
            historyIndex++;
            inputElement.value = commandHistory[historyIndex];
        } else {
            historyIndex = commandHistory.length;
            inputElement.value = "";
        }
    }
});