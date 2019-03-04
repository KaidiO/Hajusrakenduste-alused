    Vue.component('tabs', {
        template: `
    <div>
        <div class="tabs">
          <ul>
            <li v-for="tab in tabs" @click="changeTab(tab.name)" :class="{'is-active' : tab.isActive}"><a>{{tab.name}}</a></li>
          </ul>
        </div>
        <div class="tab-details">
        <slot></slot>
        </div>
    </div> 
` ,
        data(){
            return{
                tabs: [],
            }
        },
        created () {
            this.tabs = this.$children;
            console.log(this.$children);
        },
        methods: {
            changeTab(name){
                this.tabs.forEach((tab) => {
                    if (tab.name == name){
                        tab.isActive = true;
                    } else{
                        tab.isActive = false;
                    }
                });

            }
        }
    });

    Vue.component('tab', {
        template: '<div v-show="isActive"><slot></slot></div>',
        props: ['name', 'active'],
        data(){
            return{
                isActive: false
            }
        },
        mounted(){
            this.isActive = this.active;
        }
    });

    let app = new Vue({
        el: '#app',
        data: {

        },
        computer:{
        }
    });