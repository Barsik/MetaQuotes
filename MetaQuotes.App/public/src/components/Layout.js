class Layout extends HTMLElement {
  constructor() {
    super();
  }

  render() {
    this.innerHTML = `<div class="grid">
    <header><mq-header/></header>
    <main><mq-router/></main>
    <nav><mq-navbar/></nav>
    <footer><mq-footer/></footer>
    </div>`;
  }

  connectedCallback() {
    this.render();
  }
}

customElements.define("mq-layout", Layout);
