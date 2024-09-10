# CS-Server
C# based Http server that hosts an HTML webpage and serves API requests

html files must have following format:
"<"!--@{format string 1}, ..., @{format string n-1}, @{format string n}--">"
<html>
    {html content of the page}
</html>

you should only add as many format strings as you are going to use, if you add more format strings it will lead to an exception, and if you add less than enough the formatting on the client side will be broken.
