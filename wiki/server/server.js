const http = require('http');
const fs = require('fs');

http.createServer(function (req, res) {
    var path = __dirname + "/../public";
    var url = req.url.split('?')[0];
    fs.readFile(path + url, function (err, data) {
        if (err) {
            fs.readFile(path + '/index.html', function (err, index) {
                res.writeHead(200);
                res.end(index);
            });
        } else {
            res.writeHead(200);
            res.end(data);
        }
    });
}).listen(80);