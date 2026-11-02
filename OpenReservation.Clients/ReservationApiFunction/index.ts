import { HttpRequester } from './httpRequester';

const apiBaseUrl = "https://reservation.weihanli.xyz";

export function getApiPath(path: string): string {
    const normalizedPath = path.toLowerCase();
    if (normalizedPath === "/api/notice" || normalizedPath.startsWith("/api/notice/")) {
        return `/api/v1/notices${path.substring("/api/notice".length)}`;
    }
    if (normalizedPath === "/api/reservation" || normalizedPath.startsWith("/api/reservation/")) {
        return `/api/v1/reservations${path.substring("/api/reservation".length)}`;
    }
    if (normalizedPath.startsWith("/api/reservationplaces/")) {
        const resourcePath = path.substring("/api/reservationPlaces".length);
        return resourcePath.endsWith("/periods")
            ? `/api/v1/reservation-places${resourcePath.substring(0, resourcePath.length - "/periods".length)}/availability`
            : `/api/v1/reservation-places${resourcePath}`;
    }
    if (normalizedPath === "/api/reservationplaces") {
        return "/api/v1/reservation-places";
    }

    return path;
}

export function getQueryString(query: any, path: string): string {
    if (!query) {
        return "";
    }

    const parameters = Object.keys(query).map(key => {
        const parameterName = path.toLowerCase().startsWith("/api/v1/") && key === "dt" && path.endsWith("/availability")
                ? "date"
                : key;
        return `${encodeURIComponent(parameterName)}=${encodeURIComponent(query[key])}`;
    });
    return parameters.length > 0 ? `?${parameters.join("&")}` : "";
}

async function main (event: any, context: any, callback: any): Promise<object> {
    if((<string>event.path).startsWith(`${event.requestContext.path}`)){
        event.path = (<string>event.path).replace(`${event.requestContext.path}`, '');
    }

    const apiPath = getApiPath(event.path);
    const queryString = getQueryString(event.queryString, apiPath);
    let url = `${apiBaseUrl}${apiPath}${queryString}`;
    let ip:string = event.requestContext.sourceIp;
    if(ip){
        event.headers["X-Forwarded-For"] = ip;
        event.headers["X-Real-IP"] = ip;
    }
    console.log(`forwarding ${event.httpMethod} request from ${ip}`);
    
    let response = await HttpRequester.GetResponse(url, event.httpMethod, event.headers, event.body);
    console.log(`upstream response status: ${response.statusCode}`);
    
    return {
        isBase64: false,
        statusCode: response.statusCode,
        headers: response.responseHeaders,
        body: response.responseText
    };
};

exports.main_handler = main;

// (async () => {
//   process.env.NODE_TLS_REJECT_UNAUTHORIZED="0";
//   var event:any = new Object();
//   event.path = "/api/reservation";
//   event.headers = {
//       "accept":"*/*",
//       "accept-encoding":"gzip, deflate, br",
//       "cache-control":"no-cache",
//       "connection":"keep-alive",
//       "content-type":"application/json;charset=utf-8",
//       "endpoint-timeout":"15",
//       "host":"service-balxf7hr-1251288923.ap-shanghai.apigateway.myqcloud.com",
//       "pragma":"no-cache",
//       "referer":"https://servicewechat.com/wx0089396a8df636b2/devtools/page-frame.html",
//       "sec-fetch-dest":"empty",
//       "sec-fetch-site":"cross-site",
//       "sec-fetch-user":"?F",
//       "user-agent":"Mozilla/5.0 (iPhone; CPU iPhone OS 10_2 like Mac OS X) AppleWebKit/602.3.12 (KHTML, like Gecko) Mobile/14C92 Safari/601.1 wechatdevtools/1.02.1907300 MicroMessenger/7.0.4 Language/zh_CN webview/",
//       "x-anonymous-consumer":"true",
//       "x-qualifier":"$LATEST"
//   };
//   event.httpMethod = "POST";
//   event.body = '{"ReservationPlaceId":"6bbbcfa9-7b38-444d-8a80-5da9d06c62a7","ReservationPlaceName":"多媒体工作室","ReservationForDate":"2019-08-27","ReservationForTimeIds":"0","ReservationForTime":"08:00-10:00","ReservationUnit":"测试","ReservationActivityContent":"测测更健康","ReservationPersonName":"测试001","ReservationPersonPhone":"13222223333"}';
//   event.queryString = {
//       pageNumber: 1
//   };
//   event.requestContext = {
//     "httpMethod": "ANY",
//     "identity": {},
//     "path": "/reservationWxAppGateway",
//     "serviceId": "service-balxf7hr",
//     "sourceIp": "61.173.30.59",
//     "stage": "release"
//   };
//   var response = await main(event, null, null);
//   console.log(response);
// })();
