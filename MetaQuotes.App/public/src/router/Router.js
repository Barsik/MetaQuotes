import "./../components/SearchByIp.js";
import "./../components/SearchByCity.js";

class Router extends HTMLElement {
  hash = location.hash;
  routes = {
    "#ip": "<mq-search-by-ip/>",
    "#city": "<mq-search-by-city/>",
  };

  constructor() {
    super();
    this.hashChange = this.hashChange.bind(this);
    this.render = this.render.bind(this);
  }

  hashChange() {
    this.hash = location.hash;
    this.render();
  }

  render() {
    if (this.hash === "") {
      this.hash = "#ip";
    }
    this.innerHTML = this.routes[this.hash];
  }

  connectedCallback() {
    window.addEventListener("hashchange", this.hashChange);
    this.render();
  }

  disconnectedCallback() {
    window.removeEventListener("hashchange", this.hashChange);
  }
}

customElements.define("mq-router", Router);
