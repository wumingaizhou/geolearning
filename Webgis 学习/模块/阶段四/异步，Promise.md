## 异步

浏览器提供的许多功能（尤其是最有趣的那一部分）可能需要很长的时间来完成，因此需要异步完成，例如：

- 使用 [`fetch()`](https://developer.mozilla.org/zh-CN/docs/Web/API/Window/fetch) 发起 HTTP 请求
- 使用 [`getUserMedia()`](https://developer.mozilla.org/zh-CN/docs/Web/API/MediaDevices/getUserMedia) 访问用户的摄像头和麦克风
- 使用 [`showOpenFilePicker()`](https://developer.mozilla.org/zh-CN/docs/Web/API/Window/showOpenFilePicker) 请求用户选择文件以供访问

因此，即使你可能不需要经常实现自己的异步函数，你也很可能需要正确使用它们。

例如做饭时我们是按照煮饭，切菜，炒菜的顺序来的，如果按照同步的处理机制，我们在煮饭时需要等待30分钟后再工作——显然是不合理的。因此在电饭煲煮饭的时候，我们也在干其他事情——这就是异步

## Promise

在JavaScript中，`Promise` 是一种用于处理异步操作的机制。它表示一个在未来某个时间点完成或失败的操作，以及它的结果值。`Promise` 提供了一种更清晰、更合理的方式来管理异步操作，避免了传统回调方式中的“回调地狱”。

### Promise 的状态

一个 `Promise` 对象可以有以下三种状态之一：

1. **Pending**（待定）：初始状态，操作尚未完成。
2. **Fulfilled**（已实现）：操作成功完成，并返回了一个值。
3. **Rejected**（已拒绝）：操作失败，并返回了一个错误原因。

### 创建一个 Promise

可以使用 `new Promise` 构造函数来创建一个 `Promise` 对象。构造函数接受一个执行函数，这个函数有两个参数：`resolve` 和 `reject`。你可以在异步操作完成时调用 `resolve`，或者在发生错误时调用 `reject`。

```js
const myPromise = new Promise((resolve, reject) => {
  // 异步操作
  setTimeout(() => {
    const success = true; // 模拟成功或失败
    if (success) {
      resolve('Operation was successful!');
    } else {
      reject('Operation failed.');
    }
  }, 1000);
});
```

### 使用 Promise

使用 `.then()` 和 `.catch()` 方法来处理 `Promise` 的结果：

- `.then(onFulfilled, onRejected)`：处理 `Promise` 成功和失败的结果。
- `.catch(onRejected)`：处理 `Promise` 失败的结果，等同于 `.then(null, onRejected)`。
- `.finally(onFinally)`：无论 `Promise` 成功还是失败，都会执行的回调函数。

```javascript
myPromise
  .then(result => {
    console.log(result); // 'Operation was successful!'
  })
  .catch(error => {
    console.error(error); // 'Operation failed.'
  })
  .finally(() => {
    console.log('Operation completed.');
  });
```

### 示例：使用 fetch API 和 Promise

`fetch` API 是一个返回 `Promise` 的方法，常用于进行网络请求：

```javascript
fetch('https://api.example.com/data')
  .then(response => {
    if (!response.ok) {
      throw new Error('Network response was not ok');
    }
    return response.json();
  })
  .then(data => {
    console.log(data);
  })
  .catch(error => {
    console.error('There has been a problem with your fetch operation:', error);
  });
```

在这个例子中：

1. `fetch` 返回一个 `Promise`，表示网络请求。
2. 如果请求成功，`.then()` 方法的第一个回调处理响应对象。
3. 调用 `response.json()` 将响应体解析为 JSON，并返回另一个 `Promise`。
4. 解析后的 JSON 数据被传递给下一个 `.then()` 方法的回调。
5. 如果请求失败或解析失败，`.catch()` 方法捕获错误并处理。

## 使用 async 和 await

`async` 和 `await` 是 `Promise` 的语法糖，使得异步代码更易读、更像同步代码。

```javascript
async function fetchData() {
  try {
    const response = await fetch('https://api.example.com/data');
    if (!response.ok) {
      throw new Error('Network response was not ok');
    }
    const data = await response.json();
    console.log(data);
  } catch (error) {
    console.error('There has been a problem with your fetch operation:', error);
  }
}

fetchData();
```

在这个例子中：

1. `fetchData` 函数被定义为 `async`，使其能够使用 `await` 关键字。
2. `await` 在等待 `fetch` 和 `response.json()` 的 `Promise` 解析。
3. `try...catch` 结构用于处理任何可能的错误。

## 总结

- `Promise` 是用于处理异步操作的机制，提供了一种更清晰的方式来管理异步代码。
- `Promise` 有三种状态：待定（Pending）、已实现（Fulfilled）、已拒绝（Rejected）。
- 可以使用 `.then()`、`.catch()` 和 `.finally()` 方法来处理 `Promise` 的结果。
- `async` 和 `await` 提供了更易读的语法来处理 `Promise`。