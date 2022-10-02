import { geobaseService } from "./../services/geobaseService.js";

class SearchByIp extends HTMLElement {
  constructor() {
    super();
    this.search = this.search.bind(this);
    this.ipText = this.ipText.bind(this);
    this.boundEvents = this.boundEvents.bind(this);
    this.onIpChnage = this.onIpChnage.bind(this);

    this.ip = "";
    this.success = false;
    this.loading = false;
    this.result = undefined;
    this.error = undefined;
  }

  ipText() {
    return this.querySelector("#ip-text");
  }

  onIpChnage(event) {
    this.ip = event.target.value;
    this.render();
    const end = this.ipText().value.length;
    this.ipText().setSelectionRange(end, end);
    this.ipText().focus();
  }

  boundEvents() {
    this.querySelector("#ip-search").onclick = this.search;
    this.ipText().oninput = this.onIpChnage;
  }

  async search() {
    this.loading = true;
    this.success = true;
    this.result = undefined;
    this.error = undefined;
    this.render();
    const response = await geobaseService.searchByIp(this.ip);
    this.success = response.success;
    if (response.success && response.result) {
      this.result = response.result;
    } else if (!!response.error) {
      this.error = response.error;
    }

    this.loading = false;
    this.render();
  }

  renderResult() {
    return !this.loading && !!this.success && !!this.result
      ? `<div class="search-by-ip-result">
      <div><span class="search-by-ip-lable">Долгота: <span class="search-by-ip-value">${this.result.longitude}</span></span></div>
      <div><span class="search-by-ip-lable">Широта: <span  class="search-by-ip-value">${this.result.latitude}</span></span></div>
      </div>`
      : "";
  }

  renderNotFound() {
    return !this.loading && !!this.success && !this.result
      ? `<div class="not-found"><span>4😭4</span><span class="sub">ничего не найдено</span></div>`
      : "";
  }

  async render() {
    const html = `<div class="search-by">
    <div class="search-wrapper">
    <div class="search-by-label">Поиск по ip</div>
    <span class="search">
    <input id="ip-text" type="text" value="${this.ip}" />
    <button id="ip-search" ${this.ip === "" ? "disabled" : ""}>🔍</button>
    </span>
    ${!!this.loading ? `<div class="loading">Загружаем...</div>` : ""}
    ${
      !this.success && !!this.error
        ? `<div class="error">${this.error}</div>`
        : ""
    }
    ${this.renderNotFound()}
    ${this.renderResult()}
    </div>
  </div>`;

    this.innerHTML = html;
    this.boundEvents();
  }

  connectedCallback() {
    this.render();
  }
}

customElements.define("mq-search-by-ip", SearchByIp);
