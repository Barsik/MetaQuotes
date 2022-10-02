class Footer extends HTMLElement {
  constructor() {
    super();
  }

  render() {
    this.innerHTML = `<div class="footer"><a href="https://github.com/Barsik">MetaQuotes test © 2022 Lazarev</div>`;
  }

  connectedCallback() {
    this.render();
  }
}

customElements.define("mq-footer", Footer);
