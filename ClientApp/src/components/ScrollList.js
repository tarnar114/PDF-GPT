import { Card, CardBody, Flex, Text, Box, VStack } from "@chakra-ui/react";
import React from "react";
import Prompt from "./Prompt";

export default function ScrollList({messageList}) {
  return (
    <Box w="70%" h="90vh" overflowY="auto" mb={5}  >
      <VStack  spacing="4">
        {messageList.map((message,index)=>(
          <Prompt key={index} role={message.role} text={message.text}/>
        ))}
         
      </VStack>
    </Box>
  );
}
