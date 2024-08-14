## 课程目标

## 课程计划

### 第一阶段：Webgis介绍

### 第二阶段：Web基础

### 第三阶段：HTML, CSS , JS 基础

### 第四阶段：JSON and GeoJSON

- **Readings：**
  - [MDN对 JSON的介绍](https://developer.mozilla.org/zh-CN/docs/Learn/JavaScript/Objects/JSON) ：JSON是一种 JS 数据标准格式，其目的是为了便于数据在互联网交流传播，建立一种标准数据格式来有序组织数据。
  - [MDN对异步，Promise的介绍](https://developer.mozilla.org/zh-CN/docs/Learn/JavaScript/Asynchronous/Async_await)：同步处理事件不适合浏览器工作，因此产生了异步机制。但是例如`Fetch()` API是一个异步函数，但在某些场合我们需要同步处理，因此 `async` 关键字为你提供了一种更简单的方法来处理基于异步 Promise 的代码。在一个函数的开头添加 `async`，就可以使其成为一个异步函数。在异步函数中，你可以在调用一个返回 Promise 的函数之前使用 `await` 关键字。这使得代码在该点上等待，直到 Promise 被完成，这时 Promise 的响应被当作返回值，或者被拒绝的响应被作为错误抛出。
  - [GeoJSON](https://zh.wikipedia.org/wiki/GeoJSON)：地理信息使用的 JSON 格式


- **lab:**
  - lab4a：一个JSON示例
  - lab4b：使用 GeoJSON 显示地图
- **others:**
  - [DataV.GeoAtlas地理小工具系列 (aliyun.com)](https://datav.aliyun.com/portal/school/atlas/area_selector?spm=a2crr.23498931.0.0.4ad815ddOU8PfS)
  - [EasyMap (easyv.cloud)](https://map.easyv.cloud/)
  - [geojson.io | powered by Mapbox](https://geojson.io/#map=2/0/20)


### 第五阶段：地图服务

前面我们初步了解了地图API的使用以及怎么简单使用 GeoJSON 数据，但是地图API只是展示地图，他不能管理大量的地理数据，因此需要地图服务器来管理地理数据，通过地理服务器来发布地图供前端调用，最后展示。

我的学习流程：Readings的第一个部分，学习GeoSerer。先安装了 JAVA ，然后安装了 Tomcat，最后安装了部署了 Geoserver，然后继续看教程，理解了Geoserver是可以发布地图服务，你可以去调用了，然后安装了 PostGIS，利用PostGIS管理数据；

教程看到 “性能与存缓” 先把前面的消化好

- **Readings：**
  - [《GeoServer 入门与实践》 — GeoServer 0.1 文档 (osgeo.cn)](https://www.osgeo.cn/geoserver-tutorial/index.html)：GeoServer 是一个开源的服务器软件，用于发布和管理地理空间数据。它允许用户通过标准的网络协议（如 Web Map Service (WMS)、Web Feature Service (WFS) 和 Web Coverage Service (WCS)）来共享、处理和编辑地理空间数据。
- **lab:**
  -  lab5：通过PostGIS，Geoserver完成简单的地图发布