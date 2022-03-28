//重写原生Ajax
function ajax(type, url, str, timeout, success, error) {
	var xmlhttp, timer;
	if (window.XMLHttpRequest) {
		xmlhttp = new XMLHttpRequest();
	} else {
		xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
	}
	if (type == "GET") {
		xmlhttp.open(type, url + "?" + str, true);
		xmlhttp.send();
	}
	else {
		xmlhttp.open(type, url, true);
		xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
		xmlhttp.send(str);
	}
	xmlhttp.onreadystatechange = function (ev2) {
		if (xmlhttp.readyState === 4) {
			clearInterval(timer);
			if (xmlhttp.status >= 200 && xmlhttp.status < 300 || xmlhttp.status == 304) {
				success(xmlhttp);
			} else {
				error(xmlhttp);
			}
		}
	}
	if (timeout) {
		timer = setInterval(function () {
			console.log("请求超时")
			xmlhttp.abort();
			clearInterval(timer);
		}, timeout);
	}
}
