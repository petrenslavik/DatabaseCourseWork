<template>
    <router-link :to="text">
        <button class="btn">{{text}}</button>
    </router-link>
</template>

<script>
    export default {
        name: "Button",
        props:{
            text: {type:String, required: true},
        },
    }
</script>

<style scoped>
    .btn{
        border-style:none;
        cursor: pointer;
        color:white;
        display:inline-block;
        padding: .375rem .75rem;
        font-size: 1rem;
        line-height: 1.5;
        border-radius: .25rem;
        background-color: #dc3545;
    }
    .btn:hover{
        color: #fff;
        background-color: #c82333;
        border-color: #bd2130;
    }
</style>